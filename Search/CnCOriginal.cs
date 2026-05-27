using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BenchmarkDotNet.Disassemblers;
using impl_search.SetUp;
using Perfolizer.Metrology;

// Passable_Cell gegner unwichtig

namespace impl_search.Search
{
    public class CnCOriginal
    {
        // LOS path to target, if impassable then go around til there is a passable path
        // diagonal: yes

        /// <summary>
        /// How the search works:
        /// 
        /// As long as there is room to put commands in the movement command list,
	    /// then put commands in it.We build the path using the following
        /// methodology.
        /// 
        /// 1. Scan through the desired strait line path until we eiter hit an
	    /// impassable or have created a valid path.
        /// 
        /// 2. If we have hit an impassable, walk through the impassable to make
        /// sure that there is a passable on the other side.If there is not
        /// and we can not change the impassable, then this list is dead.
        /// 
        /// 3. Walk around the impassable on both the left and right edges and
	    /// take the shorter of the two paths.
        /// 
        /// 4. Taking the new location as our start location start again with
	    /// step #1.
        /// </summary>

        public Dictionary<Location, Location> cameFrom
            = new Dictionary<Location, Location>(); // to draw the grid later
        public int nodesTouched = 0;
        
        // Facingtype clock = (Facingtype)1;
        int clock = 1;
        // Facingtype counterclock = (Facingtype)(-1);
        int counterclock = -1;
        static Facingtype current_dir = new Facingtype();
        int max_moves = 200; // arbitary, but needs to be bigger if the grid is also bigger
        // int max_moves = 20000;
        bool diagonal = true; // false on crippled

        public CnCOriginal(SquareGridDiagonal graph, Location start, Location goal)
        {
            Dictionary<Location, Location> pleft = new Dictionary<Location, Location>();
            Dictionary<Location, Location> pright = new Dictionary<Location, Location>();
            Dictionary<Location, Location> which = new Dictionary<Location, Location>();
            Dictionary<Location, Location> path = new Dictionary<Location, Location>();
            // los ? -> dir direkt auf goal
            Location dir = new Location(0, 0); 
            Location current = new Location(0, 0);
            Location next = new Location(0, 0);
            int difx = goal.x - start.x;
            int dify = goal.y - start.y;
            bool passable = false;
            
            int maxlen = 200;
            // int maxlen = 20000;
            int len = 0;

            int threat = 0;
            int threat_stage = 0;
            // Risk = 0 ? weil risk wenn andere 
            int unit_threat = 10;

            bool left = false;
            bool right = false;

            

            cameFrom[start] = start;
            nodesTouched++;
            current = start;

            /*
            while (len < maxlen) {
            */

            // i hate my life
            // while (!current.Equals(goal)) {
            while (cameFrom.Count < maxlen) {
                // goto??
                top_of_list:

                if (start.Equals(goal)) break;

                // get the LOS direction first
                dir = getDirection(start, goal);

                next = Adjacent_Cell(start, dir);
                nodesTouched++;

                // normally it would check for passable by itself, but the envirement does it for you
                passable = (graph.Passable(next) && graph.InBounds(next));
                if (passable)
                {
                    // if it is passable, it is our next move
                    current = next;
                    cameFrom[next] = current;
                    
                    // Register_Cell(cameFrom, next);
                    // costSoFar[next] = newCost;
                } else
                {
                    // if it is not passable, go around til you find one
                    
                    // if unpassable is goal, break
                    if (next.Equals(goal))
                    {
                        break;
                    }

                    /*
			**	We could not move to the next cell, so follow through the
			**	impassable until we find a passable spot that can be reached.
			** Once we find a passable, figure out the shortest path to it.
			** Since we have variable passable conditions this is not as
			** simple as it used to be.  The limiter loop below allows us to
			** step through ten donuts before we give up.
			*/
                    for (int limiter = 0; limiter < 5; limiter++) {

                        /*
				**	Get the next passable position by zipping through the
				** impassable positions until a passable position is found
				**	or the destination is reached.
				*/

                        for (;;)
                        {
                            
                            // Move one step closer toward destination.
                            
                            // to stuff to move one more
                            // current new dir stuff
                            Location newDir = getDirection(next, goal);
                            next = Adjacent_Cell(next, newDir);
                            nodesTouched++;
                            /*
                        ** If the cell is passable then we have been completely
                        ** sucessful.  If the cell is not passable then continue.
                        */
                            passable = (graph.Passable(next) && graph.InBounds(next));
                            if (passable)
                            {
                                break;

                            }
                            /*
                        **	If we reached destination while in this loop, we
                        **	know that either the destination is impassible (if
                        **	we are ignoring) or that we need to up our threat
                        ** tolerance and try again.
                        */

                            if (next.Equals(goal))
                            {
                                // switch for goto top or end
                                // if
                                if(threat != -1)
                                {
                                    switch(threat_stage++)
                                    {
                                        case 0:
                                            threat = unit_threat >> 1;
                                            break;
                                        case 1:
                                            threat += unit_threat;
                                            break;
                                        case 2:
                                            threat = -1;
                                            break;

                                    }
                                    goto top_of_list;
                                }

                                // goto end
                                goto end_of_list;
                            }
                        } // for ende

                        /*
				        **	Try to find a path to the passable position by following
				        **	the edge of the blocking object in both CLOCKwise and
				        **	COUNTERCLOCKwise fashions.
				        */
                        int follow_len = maxlen + (maxlen >> 1);

                        // go around stuff left   with pleft
                        // Mem_Copy(&path, &pleft, sizeof(PathType));

                        pleft = cameFrom.ToDictionary(
                            entry => new Location(entry.Key.x, entry.Key.y),
                            entry => new Location(entry.Value.x, entry.Value.y)
                        );

                        left = Follow_Edge(start, next, pleft, dir, counterclock, threat, threat_stage, graph);
                        // path into pleft and pright

                        if (left)
                        {
                            // get if maxlen or the path is shorter
                            follow_len = MIN(maxlen, pleft.Count);
                            // not used further more, what to do with this ???
                        }

                        // go around stuff right   with pright
                        // Mem_Copy(&path, &pright, sizeof(PathType));
                        pright = cameFrom.ToDictionary(
                            entry => new Location(entry.Key.x, entry.Key.y),
                            entry => new Location(entry.Value.x, entry.Value.y)
                        );

                        right = Follow_Edge(start, next, pright, dir, clock, threat, threat_stage, graph);


                        /*
				        **	If we could find a path, break from this loop. Otherwise this
				        **	means that we have found a "hole" of passable terrain that
				        **	cannot be reached by normal means. Scan forward looking for
				        **	the other side of the "doughnut".
				        */

                        if (left || right ) break;

                        /*
				        **	If no path can be found to the intermediate cell, then
				        **	presume we have found a doughnut of some sort. Scan
				        **	forward until the next impassable is found and then
				        **	process this loop again.
				        */

                        do
                        {
                            /*
					        **	If we reached destination while in this loop, we
					        **	know that either the destination is impassible (if
					        **	we are ignoring) or that we need to up our threat
					        ** tolerance and try again.
					        */

                            if (next.Equals(goal))
                            {
                                // switch for goto top or end
                                // if
                                if (threat != -1)
                                {
                                    switch (threat_stage++)
                                    {
                                        case 0:
                                            threat = unit_threat >> 1;
                                            break;
                                        case 1:
                                            threat += unit_threat;
                                            break;
                                        case 2:
                                            threat = -1;
                                            break;

                                    }
                                    goto top_of_list;
                                }

                                // goto end
                                goto end_of_list;
                            }

                            Location newDir = getDirection(next, goal);
                            next = Adjacent_Cell(next, newDir);
                            // next = new Location(next.x + newDir.x, next.y + newDir.y);
                            nodesTouched++;

                        } while ((graph.Passable(next) && graph.InBounds(next)));


                    } // for ende

                    if (!left && !right) break;

                    /*
			        **	We found a path around the impassable locations, so figure out
			        **	which one was the smallest and copy those moves into the
			        **	path.Command array.
			        */

                    which = pleft.ToDictionary(
                            entry => new Location(entry.Key.x, entry.Key.y),
                            entry => new Location(entry.Value.x, entry.Value.y)
                        );


                    if (right)
                    {
                        which = pright.ToDictionary(
                            entry => new Location(entry.Key.x, entry.Key.y),
                            entry => new Location(entry.Value.x, entry.Value.y)
                        );
                        if (left)
                        {
                            if(pleft.Count < pright.Count)
                            {
                                which = pleft.ToDictionary(entry => new Location(entry.Key.x, entry.Key.y), entry => new Location(entry.Value.x, entry.Value.y));
                            } else
                            {
                                which = pright.ToDictionary(entry => new Location(entry.Key.x, entry.Key.y), entry => new Location(entry.Value.x, entry.Value.y));
                            }

                        }
                    }

                    /*
			        **	Record as much as possible of the shorter of the two
			        **	paths. The trailing EOL command is not copied because
			        **	this may not be the end of the find path logic.
			        */

                    // ??? copy into path, that means appending?
                    len = which.Count;
                    len = MIN(len, maxlen);
                    if (len > 0)
                    {
                        /*
                        memcpy(&path.Overlap[0], &which->Overlap[0], sizeof(LeftOverlap));
                        memcpy(&path.Command[0], &which->Command[0], len);
                        path.Length = len;
                        path.Cost = which->Cost;
                        path.LastOverlap = -1;
                        path.LastFixup = -1;
                        */
                        
                        foreach(var loca in which)
                        {
                            cameFrom[loca.Key] = loca.Key;
                        }
                        
                    }
                    else
                    {
                        break;
                    }


                } // ende if else
                start = next;
            } // ende while

            // out of while loop
            //goto stuff
            end_of_list:

            return;
            /*
            // NOT DONE
            if(diagonal)
            {
                // optimize??
                Optimize_Moves();
            }
            */
            // ALG DONE
        }

        // follow the edge of the impassable to test for a passable path around
        // return true if successful, return false if there was no path
        // 0 is right, 1 is left => search
        bool Follow_Edge(Location start, Location next, Dictionary<Location, Location> cameFrom_undone, Location olddir, int search, int threat, int threat_stage, SquareGridDiagonal graph)
        {
            bool online = true;
            int forceout = 0; // in CNC defined as false , not possible in C# , but 0 equals false
            // next is target
            int oldval = 0;
            int cellcount = 0;
            Location firstcell = new Location(-1, -1); // firstcell = -1, is out of bounds?
            Location firstdir = firstcell;
            Facingtype tmptype = Facingtype.FACING_N;

            if (!diagonal)
            {
                /*
**	The edge following algorithm doesn't "do" diagonals. Force initial facing
**	to be an even 90 degree value. Adjust it in the direction it should be
**	rotating.
*/
                // if (olddir & 0x01)
                if (((int)current_dir & 0x01) != 0)
                {
                    olddir = Next_Direction(olddir, search);
                }

            }


            // following the edge
            // newdir = olddir aber mit links oder rechts
            Location newdir = Next_Direction(olddir, search);
            // Location newdir = olddir;
            Location oldcell = start;
            Location newcell = Adjacent_Cell(oldcell, newdir);
            nodesTouched++;
            /*
	        **	Continue until we find our target, find our original starting spot,
	        **	or run out of moves.
	        */

            while (cameFrom_undone.Count < max_moves)
            {
                /*
                **	Look in all the adjacent cells to determine a passable one that
                **	most closely matches the desired direction (working in the specified
                **	direction).
                */
                newdir = olddir;
                for(;;)
                {
                    bool forcefail; // is failure forced?
                    forcefail = false;
                    if(diagonal)
                    {
                        // way more stuff
                        /*
                        **	Rotate 45/90 degrees in desired direction.
                        */
                        newdir = Next_Direction(newdir, search);
                        /*
                        **	If facing a diagonal we must check the next 90 degree location
                        **	to make sure that we don't walk right by the destination. This
                        **	will happen if the destination it is at the corner edge of an
                        **	impassable that we are moving around.
                        */
                        if(current_dir == Facingtype.FACING_NE || current_dir == Facingtype.FACING_SE || current_dir == Facingtype.FACING_SW || current_dir == Facingtype.FACING_NW)
                        // if (((int)current_dir & 1) != 0)
                        {
                            Location check;
                            check = Adjacent_Cell(oldcell, Next_Direction(newdir, search));
                            nodesTouched++;
                            if (check.Equals(next))
                            {
                                /*
                                **	This only works if in fact, it is possible to move to the
                                **	cell from the current location.
                                */
                                bool pass = (graph.Passable(next) && graph.InBounds(next));
                                if(pass)
                                {
                                    /*
                                    **	YES! The destination is at the corner of an impassable, so
                                    **	set the direction to point directly at it and then the
                                    **	scanning will terminate later.
                                    */
                                    newdir = Next_Direction(newdir, search);
                                    newcell = Adjacent_Cell(oldcell, newdir);
                                    nodesTouched++;
                                    break;
                                }
                            }
                            check = Adjacent_Cell(oldcell, newdir);
                            nodesTouched++;

                            int checkval = Point_Relative_To_Line(check.x, check.y, start.x, start.y, next.x, next.y);
                            if (checkval != 0 && !online)
                            {
                                forcefail = ((checkval ^ oldval) < 0); // bitwise XOR
                            } else
                            {
                                forcefail = false;
                            }
                            /*
                            ** The only exception to the above is when we are directly backtracking
                            ** because we could be trying to escape from a culdesack!
                            */
                            tmptype = getType(newdir);
                            if (forcefail && cameFrom_undone.Count > 0 && (((Facingtype)((int)tmptype ^ 4)).Equals(cameFrom_undone.ElementAt(cameFrom_undone.Count - 1))))
                            {
                                forcefail = false;
                            }

                        }



                    }
                    else
                    {
                        newdir = Next_Direction(newdir, ((int)search*2) );   
                    } // ENDE IF DIAGONAL

                    /*
                    **	If we have just checked the same heading we started with,
                    **	we are surrounded by impassable characters and we exit.
                    */
                    if (newdir.Equals(olddir))
                    {
                        return false;
                    }
                    /*
                    **	Get the new cell.
                    */
                    newcell = Adjacent_Cell(oldcell, newdir);
                    nodesTouched++;
                    // newcell = new Location(oldcell.x + newdir.x, olddir.y + newdir.y);
                    /*
                    **	If we found a passable position, this is where we should move.
                    */
                    // not done if
                    if (!forcefail && (graph.Passable(newcell)))
                    {
                        // stuff
                        // cameFrom_undone[newcell] = newcell; // NOT CLEAR BUT THIS IS WRONG
                        // Console.WriteLine("-------------- ! newcell x: " + newcell.x + ", y: " + newcell.y);
                        Register_Cell(cameFrom_undone, newcell);
                        break;
                    } else
                    {
                        // stuff
                        if(newcell.Equals(next))
                        {
                            forceout = 1; // is true now ??
                            break;
                        }
                    }
                } // for ende
                /*
                **	Record the direction.
                */
                if(forceout == 0) // !forceout
                {
                    /*
                    ** Mark the cell because this is where we need to be.  If register
                    ** cell fails then the list has been shortened and we need to adjust
                    ** the new direction.
                    */
                    // register cell
                    if((!Register_Cell(cameFrom_undone, newcell)))
                    {
                        // unravel loop

                        // if(!UnravelLoop(cameFrom_undone, ref newcell, ref newdir, start.x, start.y, next.x, next.y)) return false;


                        /*
                        ** Since we need to eliminate a diagonal we must pretend the upon
                        ** attaining this square, we were moving turned farther in the
                        ** search direction then we really were.
                        */
                        // newdir = Next_Direction(newdir, (((int)search * 2)));
                    }
                    /*
                    ** Find out which side of the line this cell is on.  If it is on
                    ** a side, then store off that side.
                    */
                    // ???
                    int val = Point_Relative_To_Line(newcell.x, newcell.y, start.x, start.y, next.x, next.y);
                    if (val > 0)
                    {
                        oldval = val;
                        online = false;
                    } else
                    {
                        online = true;
                    }
                    cellcount++;
                    if (cellcount == 400) // arbitrary number
                    {
                        return (false);
                    }

                } // ende if forceout 
                /*
                **	If we have found the target spot, we are done.
                */
                if (newcell.Equals(next))
                {
                    // path ende ??
                    return true;
                }
                /*
                **	If we make a full circle back to our original spot, get out.
                */
                if (newcell.Equals(firstcell) && newdir.Equals(firstdir))
                {
                    return false;
                }
                // first new setted??
                if (firstcell.x == -1)
                {
                    firstcell = newcell;
                    firstdir = newdir;
                }


                /*
                **	Because we moved, our facing is now incorrect. We want to face toward
                **	the impassable edge we are following (well, not actually toward, but
                **	a little past so that we can turn corners). We have to turn 45/90 degrees
                **	more than expected in anticipation of the pending 45/90 degree turn at
                **	the start of this loop.
                */
                if (diagonal)
                {
                    olddir = Next_Direction(newdir, (-search*3));
                } else
                {
                    olddir = Next_Direction(newdir, (-search * 4));
                }
                oldcell = newcell;

            } // while ende


            // max exhausted, abort with failure
            return false;
        }

        Location getDirection(Location start, Location goal)
        {
            Location newDir = new Location(0, 0);

            
            if (start.y < goal.y && start.x == goal.x) { newDir = new Location(0, 1); current_dir = Facingtype.FACING_N; } // N
            else if (start.y < goal.y && start.x < goal.x) { newDir = new Location(1, 1); current_dir = Facingtype.FACING_NE; } // NO
            else if (start.y == goal.y && start.x < goal.x) { newDir = new Location(1, 0); current_dir = Facingtype.FACING_E; } // O
            else if (start.y > goal.y && start.x < goal.x) { newDir = new Location(1, -1); current_dir = Facingtype.FACING_SE; } // SO
            else if (start.y > goal.y && start.x == goal.x) { newDir = new Location(0, -1); current_dir = Facingtype.FACING_S; } // S
            else if (start.y > goal.y && start.x > goal.x) { newDir = new Location(-1,-1); current_dir = Facingtype.FACING_SW; } // SW
            else if (start.y == goal.y && start.x > goal.x) { newDir = new Location(-1,0); current_dir = Facingtype.FACING_W; } // W
            else if (start.y < goal.y && start.x > goal.x) { newDir = new Location(-1, 1); current_dir = Facingtype.FACING_NW; } // NW
            
            
            return newDir;
        }
        // 0 is right, 1 is left => search
        Location Next_Direction(Location olddir, int search)
        {
            
            Facingtype tmptype = getType(olddir);
            // tmptype = (Facingtype)((int)tmptype + search);

            int count = (int)Facingtype.FACING_COUNT;
            int value = ((int)tmptype + search) % count;

            if (value < 0)
                value += count;

            tmptype = (Facingtype)value;

            Location next_dir = new Location(0, 0);

            switch (tmptype)
            {
                case Facingtype.FACING_N:
                    next_dir = new Location(0, 1);
                    current_dir = Facingtype.FACING_N;
                    break;
                case Facingtype.FACING_NE:
                    next_dir = new Location(1, 1);
                    current_dir = Facingtype.FACING_NE;
                    break;
                case Facingtype.FACING_E:
                    next_dir = new Location(1, 0);
                    current_dir = Facingtype.FACING_E;
                    break;
                case Facingtype.FACING_SE:
                    next_dir = new Location(1, -1);
                    current_dir = Facingtype.FACING_SE;
                    break;
                case Facingtype.FACING_S:
                    next_dir = new Location(0, -1);
                    current_dir = Facingtype.FACING_S;
                    break;
                case Facingtype.FACING_SW:
                    next_dir = new Location(-1, -1);
                    current_dir = Facingtype.FACING_SW;
                    break;
                case Facingtype.FACING_W:
                    next_dir = new Location(-1, 0);
                    current_dir = Facingtype.FACING_W;
                    break;
                case Facingtype.FACING_NW:
                    next_dir = new Location(-1, 1);
                    current_dir = Facingtype.FACING_NW;
                    break;
            }
            return next_dir;
        }

        // Returns the minimum of the two numbers.
        int MIN(int a, int b)
        {
            return (b < a) ? b : a;
        }

        int MAX(int a, int b)
        {
            return (b > a) ? b : a;
        }

        bool Register_Cell(Dictionary<Location, Location> path, Location cell)
        {

            if(path.ContainsKey(cell))
            {
                // Console.WriteLine("in false return");
                return false;
            }

            path[cell] = cell;

            // just logic not all stuff
            return true;
        }
        /*
        bool UnravelLoop(
    Dictionary<Location, Location> cameFrom,
    ref Location cell,
    ref Location dir,
    int sx, int sy,
    int dx, int dy)
        {
            if (cameFrom.Count < 2)
                return false;

            bool lastWasLine = false;

            // Start from the end
            int idx = cameFrom.Count - 1;

            while (idx >= 0)
            {
                var current = cameFrom.ElementAt(idx).Key;

                int checkx = current.x;
                int checky = current.y;

                int val = Point_Relative_To_Line(checkx, checky, sx, sy, dx, dy);

                if (val != 0 || lastWasLine)
                {
                    // Get previous node to compute direction
                    if (idx > 0)
                    {
                        var prev = cameFrom.ElementAt(idx - 1).Key;

                        int dxStep = current.x - prev.x;
                        int dyStep = current.y - prev.y;

                        bool isDiagonal = (dxStep != 0 && dyStep != 0);

                        if (isDiagonal)
                        {
                            cell = current;
                            dir = new Location(dxStep, dyStep);

                            // Remove everything AFTER this point
                            for (int i = cameFrom.Count - 1; i > idx; i--)
                            {
                                var keyToRemove = cameFrom.ElementAt(i).Key;
                                cameFrom.Remove(keyToRemove);
                            }

                            return true;
                        }
                    }

                    lastWasLine = !lastWasLine;
                }

                // Remove current node
                var removeKey = cameFrom.ElementAt(idx).Key;
                cameFrom.Remove(removeKey);

                idx--;
            }

            return false;
        }
        */

        int Point_Relative_To_Line(int x1, int y1, int x2, int y2, int x3, int y3)
        {
            return (int)((((long)x1 - (long)x3) * ((long)y2 - (long)y3)) - (((long)y1 - (long)y3) * ((long)x2 - (long)x3)));
        }

        Location Adjacent_Cell(Location loc, Location dir)
        {
            return new Location(loc.x + dir.x, loc.y + dir.y);
        }


        Facingtype Opposite(Facingtype face)
        {
            return ((Facingtype)((int)face ^ 4));
        }

        Location Opposite (Location location)
        {
            return new Location(location.x *(-1), location.y * (-1));
        }

        void Optimize_Moves()
        {
            
        }

        Facingtype getType(Location dir)
        {
            Facingtype type =Facingtype.FACING_N;
            if(dir.x == 0 && dir.y == 1){ type = Facingtype.FACING_N; }
            else if(dir.x == 1 && dir.y == 1){ type = Facingtype.FACING_NE; }
            else if(dir.x == 1 && dir.y == 0) { type = Facingtype.FACING_E; }
            else if (dir.x == 1 && dir.y == -1) { type = Facingtype.FACING_SE; }
            else if(dir.x == 0 && dir.y == -1) { type = Facingtype.FACING_S; }
            else if (dir.x == -1 && dir.y == -1) { type = Facingtype.FACING_SW; }
            else if(dir.x == -1 && dir.y == 0) { type = Facingtype.FACING_W; }
            else if(dir.x == -1 && dir.y == 1) { type = Facingtype.FACING_NW; }

            return type;
        }
    }
}
