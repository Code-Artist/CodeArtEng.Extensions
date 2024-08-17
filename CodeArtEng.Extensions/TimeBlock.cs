using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;

namespace System
{
    public class TimeBlock
    {
        /// <summary>
        /// Default constructor 
        /// </summary>
        public TimeBlock() { }

        /// <summary>
        /// Create new instance with start time and end time
        /// </summary>
        /// <param name="startTime"></param>
        /// <param name="endTime"></param>
        public TimeBlock(DateTime startTime, DateTime endTime)
        {
            StartTime = startTime;
            EndTime = endTime;
        }

        /// <summary>
        /// Create new instance with start time and durations.
        /// </summary>
        /// <param name="startTime"></param>
        /// <param name="durationInSeconds"></param>
        public TimeBlock(DateTime startTime, double durationInSeconds)
        {
            StartTime = startTime;
            DurationInSeconds(durationInSeconds);
        }


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

        /// <summary>
        /// Gets or sets a user-defined object that provides additional information about this TimeBlock.
        /// </summary>
        /// <remarks>
        /// This property can be used to associate custom data with the TimeBlock instance.
        /// It is particularly useful for scenarios where you need to attach metadata or 
        /// related objects to the TimeBlock without extending the class.
        /// </remarks>
        /// <example>
        /// timeBlock.Tag = new { Category = "Meeting", Priority = 1 };
        /// </example>
        public object Tag { get; set; }


        /// <summary>
        /// Shifts both the StartTime and EndTime of the TimeBlock by the specified number of seconds.
        /// </summary>
        /// <param name="totalSeconds">The number of seconds to shift the TimeBlock. Can be positive or negative.</param>
        /// <returns>The current TimeBlock instance with updated StartTime and EndTime.</returns>
        /// <remarks>
        /// Positive values shift the TimeBlock forward in time, while negative values shift it backward.
        /// </remarks>
        public TimeBlock Offset(double totalSeconds)
        {
            StartTime = StartTime.AddSeconds(totalSeconds);
            EndTime = EndTime.AddSeconds(totalSeconds);
            return this;
        }

        /// <summary>
        /// Shifts both the StartTime and EndTime of the TimeBlock by the specified TimeSpan.
        /// </summary>
        /// <param name="duration">The TimeSpan to shift the TimeBlock. Can be positive or negative.</param>
        /// <returns>The current TimeBlock instance with updated StartTime and EndTime.</returns>
        /// <remarks>
        /// Positive durations shift the TimeBlock forward in time, while negative durations shift it backward.
        /// </remarks>
        public TimeBlock Offset(TimeSpan duration) { return Offset(duration.TotalSeconds); }

        /// <summary>
        /// Extends the EndTime of the TimeBlock by the specified number of seconds.
        /// </summary>
        /// <param name="totalSeconds">The number of seconds to extend the TimeBlock. Can be positive or negative.</param>
        /// <returns>The current TimeBlock instance with an updated EndTime.</returns>
        /// <remarks>
        /// Positive values increase the duration of the TimeBlock, while negative values decrease it.
        /// The StartTime remains unchanged.
        /// </remarks>
        public TimeBlock Extend(double totalSeconds)
        {
            EndTime = EndTime.AddSeconds(totalSeconds);
            return this;
        }

        /// <summary>
        /// Extends the EndTime of the TimeBlock by the specified TimeSpan.
        /// </summary>
        /// <param name="duration">The TimeSpan to extend the TimeBlock. Can be positive or negative.</param>
        /// <returns>The current TimeBlock instance with an updated EndTime.</returns>
        /// <remarks>
        /// Positive durations increase the duration of the TimeBlock, while negative durations decrease it.
        /// The StartTime remains unchanged.
        /// </remarks>
        public TimeBlock Extend(TimeSpan duration) { return Extend(duration.TotalSeconds); }
    }
}
