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

The algorithm has been implemented together with both command line and UI interfaces.
With the aid of profiling, I reduced the initial running time of ~40 minutes down ~20 seconds, optimizing memory allocations and parallelism.
I plan to inspect further improvements that may be achievable using value type optimizations possible with the latest advancements of C#.
