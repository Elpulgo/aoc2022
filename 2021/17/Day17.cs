
using System;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using Aoc2022;

namespace Aoc2022.Aoc2021
{
    internal class Day17 : BaseDay
    {
        public Day17(bool shouldPrint): base(2021, nameof(Day17), shouldPrint) {
        }

      private string Regex = @"[0-9-]{1,4}";
        
        private int _xMax = 0;
        private int _xMin = 0;
        private int _yMax = 0;
        private int _yMin = 0;

        public override void Execute()
        {
            Setup();

            base.StartTimerOne();
            base.StartTimerTwo();

            var velocities = Enumerable.Range(1, _xMax + 1).Where(probableVX => 
                    (probableVX * (probableVX + 1) / 2) > _xMin && 
                    (probableVX * (probableVX + 1) / 2) < _xMax)
                .ToList();

            var maxY = 0;
            var allWithinTargetVY = new List<int>();

            // Brute force, ugly but true!
            foreach (var velocity in velocities)
            for(var y = 0; y < 1000; y++){
                var position = (X: 0, Y: 0);
                
                var velocityX = velocity;
                var velocityY = y;

                var probeRoundMaxY = 0;

                while(!IsWithinTarget(position)){
                    (velocityX, velocityY) = Step(ref position, velocityX, velocityY);

                    if(IsBeyondTarget(position, velocityX)){
                        probeRoundMaxY = 0;
                        break;
                    }

                    if(position.Y > probeRoundMaxY)
                        probeRoundMaxY = position.Y;
                }

                if(probeRoundMaxY > 0)
                    allWithinTargetVY.Add(y);

                if(probeRoundMaxY > maxY)  
                    maxY = probeRoundMaxY;
            }

            base.StopTimerOne();
            FirstSolution(maxY.ToString());

            var targetHitsCount = 0;
            var vyRange = allWithinTargetVY.Concat(Enumerable.Range(_yMin, Math.Abs(_yMin) + 1)).Distinct().ToList();

            foreach (var vx in Enumerable.Range(velocities.OrderBy(o => o).First() - 1, _xMax + 1))
            foreach (var vy in vyRange)
            {
                var position = (X: 0, Y: 0);
                
                var velocityX = vx;
                var velocityY = vy;

                var hitTarget = true;

                while(!IsWithinTarget(position)){
                    (velocityX, velocityY) = Step(ref position, velocityX, velocityY);

                    if(IsBeyondTarget(position, velocityX)){
                        hitTarget = false;
                        break;
                    }
                }

                if(hitTarget)
                    targetHitsCount++;
            }

            base.StopTimerTwo();
            SecondSolution(targetHitsCount.ToString());
        }

         private static int StepsRequired(int velocity, int target){
            int steps = 0;
            while(target >= 0){
                target -= velocity;
                velocity--;
                steps++;
            }

            return steps;
        }

        private bool IsWithinTarget((int X, int Y) position)
        {
            if ((position.X <= _xMax && position.X >= _xMin) && 
                (position.Y <= _yMax && position.Y >= _yMin))
                return true;

            return false;
        }

        private bool IsBeyondTarget((int X, int Y) position, int velocityX)
        {
            if(position.Y < _yMin)
                return true;

            if(position.X > _xMax || (position.X < _xMin && velocityX == 0))
                return true;

            return false;
        }

        // The probe's x position increases by its x velocity.
        // The probe's y position increases by its y velocity.
        // Due to drag, the probe's x velocity changes by 1 toward the value 0; that is, it decreases by 1 if it is greater than 0, increases by 1 if it is less than 0, or does not change if it is already 0.
        // Due to gravity, the probe's y velocity decreases by 1.
        private static (int velocityX, int velocityY) Step(ref (int X, int Y) position, int velocityX, int velocityY)
        {
            position.X += velocityX;
            position.Y += velocityY;

            velocityX = velocityX switch
            {
                > 0 => velocityX -= 1,
                < 0 => velocityX += 1,
                _ => velocityX
            };

            velocityY--;

            return (velocityX, velocityY);
        }

        private void Setup(){
            var input = ReadInput().First();

            var split = input.Split(",", StringSplitOptions.RemoveEmptyEntries);

            var xHighToLow = new Regex(Regex).Matches(split.First()).Select(s => Convert.ToInt32(s.Value)).OrderByDescending(o => o).ToList();
            _xMax = xHighToLow.First();
            _xMin = xHighToLow.Last();

            var yHighToLow = new Regex(Regex).Matches(split.Last()).Select(s => Convert.ToInt32(s.Value)).OrderByDescending(o => o).ToList();
            _yMax = yHighToLow.First();
            _yMin = yHighToLow.Last();
        }
    }
}