# Stuck
___

## Downloads

+ Build of submitted version with .exe: https://drive.google.com/file/d/1ObsvKLh9P1WcjJpzwbDJSMRwKji8x074/view?usp=sharing
+ WebGL build of submitted version: https://drive.google.com/file/d/1cMZLVgclUeoRQdzJb_PMpEIVHzntiBT0/view?usp=sharing
+ Presentation for this game (with images and videos) in .pptx: https://drive.google.com/file/d/1czVoh86GSE7WLKzdtzRFP9nco5wJ1jfc/view?usp=sharing 

## Outline

+ [About](#about)
+ [IMPORTANT](#important)
+ [Version](#version)
+ [Game](#game)
    - [Control](#control)
    - [UI](#ui)
    - [Story](#story)
    - [Goal and description](#goal-and-description)
    - [Enviroment](#enviroment)
+ [TO DO and ideas](#to-do-and-ideas)
+ [Bugs](#bugs)
+ [GameJam experiences](#gamejam-experiences)
+ [Author](#author)
+ [Credits](#credits)

___

![From game](https://github.com/vojone/IZHV_GameJam/blob/master/Screenshots/InGame1.png)
![From game](https://github.com/vojone/IZHV_GameJam/blob/master/Screenshots/InGame2.png)

## About

Simple 2D dungeon crawler game with roguelike and adventure elements. The demoversion was created during Game Jam in Introduction to Game Development course in december 2021. My theme for this Game Jam was **Stuck in a loop**.

## Important

It is only demo version, so there is only one handmade level to demostrate game concept. Therefore there is only one simple level. If you leave it you should win and see text "You are not stuck". 

Hint: Levers in main room (in green circles on picture below) are connected to NOR logical function and the **off state demonstrates lever on left side**.  

![Hint](https://github.com/vojone/IZHV_GameJam/blob/master/Screenshots/Hint.png)


## Version

v0.9 - Demo

___

## Game

### Control

`W`,`A`,`S`,`D`    Movement

`E`     Interaction (interactive element -time checkpoint, lever or button must be nearby your character) 

`MOUSE`    Aiming 

`LEFT MOUSE BUTTON`    Hold to charge and release it to shoot spell (longer holding results in more powerfull spell, there is also minimal charge level for spell shooting). In menu you can tap the buttons by clicking on it. Yeah **mouse is mandatory** so far.

`Esc`     Pause the game, if it was started and show main menu. Next pressing Esc will resume the game.

### UI
There are two UI modes:

1. Menu

Control state of game or show other information by clicking buttons.

![From game](https://github.com/vojone/IZHV_GameJam/blob/master/Screenshots/MainMenu.png)

2. InGame UI

Shows you remaining time that you have (left down corner) and remaining HP of your character (hearts in left down corner)


### Story

You are a wizard, that likes experiments with magic. One day one of your spells got out of your hands and blocked you in a time loop. To get rid of this curse you should (perhaps) leave your (wizards) cursed house. 

### Goal and description

Survive and leave your house (dungeon). It wont be so easy, because your there are a lot of security precautions, such as puzzles creatures and indestructible walls and doors (originally it was prepared for univited guests - other envious and worse wizards). Some of these things can hurt you and you have naturally limited health. So, be carefull!

And, of course there is still the **Time loop**, which returns your position. However, there are strange places where you can create temorary **time check point**. After that the time loop will return you to this checkpoint. You can have only one temporary time checkpoint active and it can be disabled by some lever or button. In this case (your temporary time checkpoint is disabled) is used your default time checkpoint  (the red one), that cannot be disabled. Other creatures and elements are **not affected** by the time loop. Yeah, returning can little bit frustrating... But maybe you should utilize it?! 

### Enviroment

You are in wizards mansion. Don't forget!

+ There are **time checkpoints** (= floor with blue or red circles), levers, buttons. With this elements you can *interact*. Levers and buttons can open/close linked doors or (dis)able time check point.
+ Red time checkpoint is main and cannot be disabled (there is only one).
+ There are also **walls** and other collideable obstacles. They are in most cases solid (maybe there is a secret path or something... I don't know it's a wizards house, there can be everything)
+ **Doors** can be opened or closed by linked lever or buttons. To one door can be linked more these elements and there can be something like "logical function", that opens the door.
+ **Enemies** (green slimes) can follow you if you hit them or if you abstain in their neighbourhood. If they are close enough, they can hit you. 
+ Some of enviroment elements has only **decorative** purpose.

### Enjoy!

___

## TO DO and ideas

+ More spells with various powers, spell selecting
+ Items and inventory (potions, sand to watches)
+ Returning to position where player was not only to position of checkpoints.
+ Light
+ Visible area
+ More enemies with various abilities
+ Procedurarly generated dungeon
+ Better emeny AI (navigation)
+ Decorative elements in scene
+ Animation improvals (make the smoother and better timed)
+ Destroyable obstacles (e. g. wooden wall that can be destroyed by fireball or something)

## Bugs 

There are probably a lot of bugs.

## GameJam experiences

It was my first GameJam. Here are the most important thing I realized (you can ignore it, a lot of problems are only mine problems, I wrote it especially for me,  I don't wanna fail twice) :

+ The time limit is cruel. It doesn't matter how much time you have everytime it is not enough. There is NO TIME for polishing and perfectionism.
+ If you are stuck (hehe), try to solve something else or completely ignore it (it is not so important).
+ Wiritting game concepts, ideas, elements is veeeeery usefull, although it takes time. It helps you to prioritize important things. I spent most of time on details (particles, animation, drawing beautifull assets...)
+ Before it starts, make sure that your development tools works as you expected. Setup .gitignore and git repository to avoid uploading bilion files.
+ If it is possible (if there is no limitation in licence or in rules of GameJam), try to find Assets and be satisfied with that  (if it is at least a little possible) or modify it. E. g. Drawing something from start takes a lot of time. But, as I said, it depends... 

## Authors

Vojtěch Dvořák (vojone)

**Used Assets, inspiration and ideas, that are not mine are mentioned below.**

## Credits

### Used Assets

+ www.0x72.itch.io/dungeontileset-ii  (Used one of tilesets with floor, walls, creatures... Thank you!)
+ www.dafont.com/vcr-osd-mono.font (Used font. Thanks!)

The rest of assets are created by me or I modified the used assets (it should be OK due to licences).

### Sources of know-how and ideas

+ www.piskelapp.com/p/create/sprite (was very usefull for creating Assets)
+ www.learn.unity.com
+ Exercises from IZHV course (see www.github.com/vojone?tab=repositories).
+ IZHV course web page
+ Of course www.docs.unity3d.com/Manual/index.html
+ Unity forum https://answers.unity.com
+ www.raywenderlich.com/23-introduction-to-the-new-unity-2d-tilemap-system (some tips&tricks for tilemap games in Unity)
+ The game is inspired by roguelike games such as Pixel Dungeon or RPGs like Diablo

