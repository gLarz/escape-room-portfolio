using System;

class Program
{
    static char[,] room;
    static int roomWidth;
    static int roomHeight;
    static int playerX;
    static int playerY;
    static int keyX;
    static int keyY;
    static int doorX;
    static int doorY;
    static bool hasKey = false;
    public static bool doorLocked = true;

    static void Main()
    {
        Typewrite("Welcome to the Escape Room!\n");
        Typewrite("Instructions: Use arrow keys to move the player\n");
        Typewrite("Collect the key, unlock the door and leave the room.\n\n");
        Typewrite("Press any key to continue and to set the room dimensions\n\n");
        Console.ReadKey(true); // Doesnt display the key
        // Get room dimensions from the user
        GetRoomDimensions();
        // Initialize the game room
        InitializeRoom();
        // Randomly place player, key and door in the room
        PlacePlayer();
        PlaceKey();
        PlaceDoor();

        // Main game loop
        while (true)
        {
            Console.Clear();
            DrawRoom();
            MovePlayer();
        }

        static void Typewrite(string message)
        {
            for (int i = 0; i < message.Length; i++)
            {
                Console.ForegroundColor = ConsoleColor.Green;
                Console.Write(message[i]);
                System.Threading.Thread.Sleep(40);
            }
        }

        static void GetRoomDimensions()
        {
            bool validInput = false;
            while (!validInput)
            {
                Typewrite("For the width, type a number between 2 and 100: ");
                if (int.TryParse(Console.ReadLine(), out roomHeight) && roomHeight > 2)
                {
                    validInput = true;
                }
                else
                {
                    Typewrite("Invalid input. Please enter a positive number.");
                }
            }

            validInput = false;
            while (!validInput)
            {
                Typewrite("\nFor the height, type a number between 2 and 50: ");
                if (int.TryParse(Console.ReadLine(), out roomWidth) && roomWidth > 2)
                {
                    validInput = true;
                }
                else
                {
                    Typewrite("Invalid input. Please enter a positive number.");
                }
            }
        }

        static void InitializeRoom()
        {
            room = new char[roomWidth, roomHeight];
            for (int i = 0; i < roomWidth; i++)
            {
                for (int j = 0; j < roomHeight; j++)
                {
                    if (i == 0 || i == roomWidth - 1 || j == 0 || j == roomHeight - 1)
                    {
                        room[i, j] = '█'; // Walls
                    }
                    else
                    {
                        room[i, j] = ' '; // No flooring, I didn't like the flooring
                    }
                }
            }
        }

        static void DrawRoom()
        {
            Console.ForegroundColor = ConsoleColor.Red;
            for (int i = 0; i < roomWidth; i++)
            {
                for (int j = 0; j < roomHeight; j++)
                {
                    Console.Write(room[i, j]);
                }
                Console.WriteLine();
            }
        }

        static void PlacePlayer()
        {
            Random random = new Random();
            playerX = random.Next(1, roomWidth - 1);
            playerY = random.Next(1, roomHeight - 1);
            room[playerX, playerY] = 'P';
        }

        static void PlaceKey()
        {
            Random random = new Random();
            keyX = random.Next(1, roomWidth - 1);
            keyY = random.Next(1, roomHeight - 1);
            room[keyX, keyY] = 'K';
        }

        static void PlaceDoor()
        {
            Random random = new Random();
            int side = random.Next(4); // 0: top, 1: right, 2: bottom, 3: left, walls
            switch (side)
            {
                case 0: // Top wall
                    doorX = random.Next(1, roomWidth - 1);
                    doorY = 0;
                    break;
                case 1: // Right wall
                    doorX = roomWidth - 1;
                    doorY = random.Next(1, roomHeight - 1);
                    break;
                case 2: // Bottom wall
                    doorX = random.Next(1, roomWidth - 1);
                    doorY = roomHeight - 1;
                    break;
                case 3: // Left wall
                    doorX = 0;
                    doorY = random.Next(1, roomHeight - 1);
                    break;
            }
            doorLocked = true;
            room[doorX, doorY] = 'D';
        }

        static void MovePlayer()
        {
            ConsoleKeyInfo keyInfo = Console.ReadKey();
            int newX = playerX;
            int newY = playerY;

            switch (keyInfo.Key)
            {
                case ConsoleKey.UpArrow:
                    newX--;
                    break;
                case ConsoleKey.DownArrow:
                    newX++;
                    break;
                case ConsoleKey.LeftArrow:
                    newY--;
                    break;
                case ConsoleKey.RightArrow:
                    newY++;
                    break;
                default:
                    return; // Ignore other keys
            }

            // Check if the new position is valid
            if (newX >= 0 && newX < roomWidth && newY >= 0 && newY < roomHeight)
            {
                char newPosition = room[newX, newY];

                if (newPosition == '█')
                {
                    // Hit a wall, can't move there
                    return;
                }
                else if (newPosition == 'K')
                {
                    // Collect the key
                    hasKey = true;
                    room[newX, newY] = ' ';
                    Console.Beep();
                }
                if (newPosition == 'D' && !hasKey)
                {
                    return;
                }
                if (newPosition == 'D' && hasKey && doorLocked)
                {
                    doorLocked = false;
                }
                if (newPosition== 'D' && !doorLocked)
                {
                    Console.ForegroundColor = ConsoleColor.Magenta;
                    Typewrite("\n\nCongratulations! You've escaped!\n");
                    Typewrite("Press 'R' to replay or 'ESC' key to exit\n\n");
                    room[doorX, doorY] = '#';
                    playerX = roomWidth / 2;
                    playerY = roomHeight / 2;
                    ConsoleKeyInfo replayKey;
                    do
                    {
                        replayKey = Console.ReadKey(true); // Read the key without displaying it
                    } while (replayKey.Key != ConsoleKey.R && replayKey.Key != ConsoleKey.Escape);

                    if (replayKey.Key == ConsoleKey.R)
                    {
                        GetRoomDimensions();
                        InitializeRoom();
                        PlacePlayer();
                        PlaceKey();
                        PlaceDoor();
                        doorLocked = true; // Lock the door again for the next game
                    }
                    else
                    {
                        Environment.Exit(0);
                    }
                    // Clear the console after the player makes a choice
                    Console.Clear();
                }
                else
                {
                    // Move to the new position
                    room[playerX, playerY] = ' ';
                    playerX = newX;
                    playerY = newY;
                    room[playerX, playerY] = 'P';
                }
            }


        }

    }
}


        

