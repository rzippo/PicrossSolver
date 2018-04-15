# Introduction

Aim of this project is to build a set of applications to solve picross puzzles, also known as nonograms, gliddlers, etc.
The project follows my previous experience with [SudokuSolver](https://github.com/rzippo/SudokuSolver), with a new kind of puzzle as challenge and set of technology to test.

# Project structure and tools used

This time I added the idea of both a command line and a GUI applications, both built on a .NET Standard library which provides data structures and solving algorithm.
Puzzle are defined in json instead of a custom syntax, and unit tests are used during debug process to isolate bugs and test against regression after they are fixed.
Event propagation is used to efficiently propagate changes from individual cells only to the related high-level structures.

# About the algorithm

As in [SudokuSolver](https://github.com/rzippo/SudokuSolver), the solving algorithm is an implementation of the [backtracking algorithm](https://en.wikipedia.org/wiki/Backtracking).

At inizialization time, all possible solutions are computed for each line, which are then filtered down each time a cell is set.
The first step is then to compute the intersections of the candidates for each line so to find cells that are for sure to be set.
Then in the deduction phase lines with only one candidate solution left are identified and set.
When no further deduction is possible, speculation is used.

# Progress so far

I implemented the solving algorithm and tested with a couple of examples, with aim to add more to find eventual bugs.
I implemented the command line application, which is being used for the live tests, while the GUI is for now on hold.
As for performance, single threading results in really long computations that can take up to 40 minutes. Further improvements I'm planning include parallelization and, if possible, vectorization.