# Overview

As a software engineer, my goal with this project was to expand my expertise in modern desktop application development using C#. I wanted to master OOP principles, memory structures, and graphical user interfaces while building a practical, interactive software project from scratch.

I developed a modern desktop Tic-Tac-Toe application using C# and Windows Forms. The software features a dark-themed visual interface, real-time board state validation, interactive turn-based play between two players, and automatic score persistence using file I/O operations.

The primary purpose of writing this software was to gain hands-on experience with the C# language syntax and runtime features. Specifically, I wanted to learn how to manipulate memory layouts using custom structures and explicit struct overlays, build event-driven UI controls, handle file persistence, and structure maintainable, modular C# code.

[Software Demo Video](https://www.youtube.com/watch?v=43m3yV1FWjg)


# Development Environment

- IDE / Code Editor: Visual Studio Code (VS Code)
- Extensions Used: C# Dev Kit, C# Extension (by Microsoft)
- Build Tool & CLI: .NET SDK 8.0 Command-Line Interface (dotnet CLI)
- Programming Language: C# 12 / .NET 8.0
- Libraries & Frameworks:
- System.Windows.Forms (for creating UI forms, grid layouts, labels, and button event handlers)
- System.Drawing (for custom color palettes, fonts, and control styling)
- System.IO (for reading and writing score data to local storage)
- System.Runtime.InteropServices (for simulating explicit memory overlays/unions)


# Useful Websites

- [Microsoft Learn: C# Documentation](https://learn.microsoft.com/en-us/dotnet/csharp/)
- [Microsoft Learn: Windows Forms](https://learn.microsoft.com/en-us/dotnet/desktop/winforms/)
- [C# StructLayoutAttribute Class](https://learn.microsoft.com/en-us/dotnet/api/system.runtime.interopservices.structlayoutattribute)


# Future Work

- Add an optional AI opponent utilizing the Minimax algorithm for single-player mode.
- Implement animated transitions or win-line highlight effects when a player places three in a row.
- Add dynamic player name input fields and a full historical scoreboard log instead of just tracking total wins.
