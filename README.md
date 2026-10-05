# Match-3

![Match-3](/Match3.png?raw=true "Match-3")

A programming test in which I had to increment a very basic match-3 game

### Overview
My main focus on my increments and changes was following the project's general architecture of **MVC** and attempting to **modularize** and make the project **as maintainable as possible**, with the mindset I'd use if I was part of the team and **developing a project for years to come**.

### My changes

- Used the [Strategy](https://refactoring.guru/design-patterns/strategy) design pattern to create new match shapes
  - [4, 5, 6 and 7+ tile lines](https://github.com/SirBernhart/fork-desafio-match-3/blob/main/Assets/Project/Script/Controllers/MatchControllers/LineMatchPatternStrategy.cs)
  - [Square](https://github.com/SirBernhart/fork-desafio-match-3/blob/main/Assets/Project/Script/Controllers/MatchControllers/SquareMatchPatternStrategy.cs)
  - [MatchController](https://github.com/SirBernhart/fork-desafio-match-3/blob/main/Assets/Project/Script/Controllers/MatchControllers/MatchController.cs) that finds all matches (using the Strategies) and returns a list with them
- Implemented modular match type bonus classes, which can be used by any match pattern
  - 
- Created VFX prefabs and scripts for each match type bonus
  - Most animations made using DOTween in View classes, and another with Unity's Animator
- Implemented tile selection animation with DOTween

Developed in Unity 6000.3.16f1

## Links
- https://unity.com/releases/editor/archive
