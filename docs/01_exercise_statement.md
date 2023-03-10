## Exercise statement

The company X wants to develop an automower for square surfaces.

The mower can be programmed to go throughout the whole surface. Mower's position is represented by coordinates (X,Y) and a characters indicate the orientation
according to cardinal notations (N,E,W,S). The lawn is divided in grid to simplify navigation.
For example, the position can be 0,0,N, meaning the mower is in the lower left of the lawn, and oriented to the north.

To control the mower, we send a simple sequence of characters. Possibles characters are *L*,*R*,*F*. *L* and *R* turn the mower at 90° on the left or right without
moving the mower. *F* means the mower move forward from one space in the direction in which it faces and without changing the orientation.

If the position after moving is outside the lawn, mower keep it's position, retains its orientation and go to the next command.

We assume the position directly to the north of (X,Y) is (X,Y+1).

To program the mower, we can provide an input file constructed as follows:
- The first line correspond to the coordinate of the upper right corner of the lawn. The
bottom left corner is assumed as (0,0). 

The rest of the file can control multiple mowers deployed on the lawn. Each mower has 2 next lines :
- The first line give mower's starting position and orientation as "X Y O". X and Y being the position and O the orientation.
- The second line give instructions to the mower to go throughout the lawn.

Instructions are characters without spaces.

Each mower move sequentially, meaning that the second mower moves only when the first has fully performed its series of instructions.

When a mower has finished, it give the final position and orientation.

### Example
input file
```
5 5
1 2 N
LFLFLFLFF
3 3 E
FFRFFRFRRF
```
result
```
1 3 N
5 1 E
```