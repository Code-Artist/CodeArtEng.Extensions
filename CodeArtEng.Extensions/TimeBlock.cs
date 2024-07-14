using System;
using System.Collections.Generic;
using System.Linq;

namespace CodeArtEng.Extensions
{
    public class TimeBlock
    {
        /// <summary>
        /// Represents the start time of the time block. Default is DateTime.MinValue.
        /// </summary>
        public DateTime StartTime { get; set; } = DateTime.MinValue;
        /// <summary>
        /// Represents the end time of the time block. Default is DateTime.MinValue.
        /// </summary>
        public DateTime EndTime { get; set; } = DateTime.MinValue;
        /// <summary>
        /// Gets the duration between StartTime and EndTime. 
        /// Setting this property adjusts the EndTime based on the duration.
        /// </summary>
        public TimeSpan Duration
        {
            get => EndTime - StartTime;
            set => EndTime = StartTime.AddTicks(value.Ticks);
        }

        /// <summary>
        /// A list of child TimeBlock objects. Default is null.
        /// </summary>
        public List<TimeBlock> Childrens { get; set; } = null;
        /// <summary>
        /// A boolean indicating if there are any child TimeBlock objects.
        /// </summary>
        public bool HasChildren => Childrens != null;

        /// <summary>
        /// Sets the duration of the time block in seconds.
        /// </summary>
        ///<param name="value">The duration in seconds.</param>
        public void DurationInSeconds(double value) { Duration = TimeSpan.FromSeconds(value); }

        /// <summary>
        /// Groups an array of TimeBlock objects into a list of TimeBlock groups, 
        /// merging overlapping time blocks and returning the result as an array.
        /// </summary>
        ///<param name="items">An array of TimeBlock objects to be grouped.</param>
        ///<returns>An array of grouped TimeBlock objects.</returns>
        public static TimeBlock[] Groups(params TimeBlock[] items)
        {
            List<TimeBlock> results = new List<TimeBlock>();
            if (items == null) return null;
            if (items.Length == 0) return results.ToArray();

            List<TimeBlock> inputs = items.OrderBy(n => n.StartTime).ToList();

            //Add first object
            TimeBlock ptrBlock = inputs.First().Copy();
            ptrBlock.Childrens = new List<TimeBlock> { inputs.First() };
            results.Add(ptrBlock);

            foreach (TimeBlock i in inputs.Skip(1))
            {
                if (i.StartTime < ptrBlock.EndTime)
                {
                    //Continue current block
                    ptrBlock.EndTime = i.EndTime;
                }
                else
                {
                    //Create new block
                    results.Add(ptrBlock = i.Copy());
                    ptrBlock.Childrens = new List<TimeBlock>();
                }
                ptrBlock.Childrens.Add(i);
            }
            return results.ToArray();
        }

        /// <summary>
        /// Determines whether this TimeBlock overlaps with another TimeBlock.
        /// </summary>
        /// <param name="other">The TimeBlock to check for overlap.</param>
        /// <returns>
        /// true if the TimeBlocks overlap; otherwise, false.
        /// </returns>
        /// <remarks>
        /// Two TimeBlocks are considered to overlap if the start time of one
        /// is earlier than the end time of the other, and vice versa.
        /// </remarks>
        public bool Overlaps(TimeBlock other)
        {
            return StartTime < other.EndTime && other.StartTime < EndTime;
        }
    }
}
