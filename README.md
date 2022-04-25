# DALI_Unity_Project
A short Unity game made as part of an application to the DALI Lab

# Running the game
Once opening the project in Unity, open the scene titled "Game" in the Scenes folder, and you can hit play to run the game.
While I think it is most fun to go into the game blind, I'll describe it briefly below.

The goal of the game is simple: Don't touch anything red. You play as a white square, moving around a larger sphere. There is also a red square,
who follows you endlessly. Every second, a red cylinder spawns in the sky around you and moves towards the sphere you are moving on. 
As time progresses, more and more of the sphere is covered by these cylinders, and the available ground
you have to walk on diminishes. Touching one of the red cylinders, or the red square chasing you, will end the game. The longer you live, the better you do.

Every now and then, a blue cylinder will spawn. Touching it will grant you the brief ability to shoot and destroy some of the red cylinders.

# How it works
The most interesting bit of this project was working with the "projectile spawner," the system that spawns the red cylinders (referred to as projectiles in the code) 
around the main sphere. Each second, a random point around the sphere is taken, and then a ray is cast from that point to the sphere to check
if anything is in the way. If nothing is in the way, a projectile is spawned at that location and it begins to move towards the sphere. An invisible collider
is also spawned at where that projectile is going to land, to prevent the game from trying to spawn any others there in the future.
As the game continues, the projectile spawner creates more and more projectiles around the sphere, occasionally having to check different locations
a number of times to make sure it never spawns two on top of each other.

Implementing this system let me experiment with object pooling, and how to best optimize how many times the projectile spawner would need to check a location 
(if there were already many projectiles on the sphere, then the spawner might need to check 50 different locations before it found an unblocked path).

# Final thoughts
I'm happy with this final result, and think that it's pretty fun to play. The small group of people I gave it to playtest all seemed to enjoy it, which tells
me I'm doing something right as a game designer. There are certainly ways to improve it, but as a prototype, I'm very happy with how it turned out.
