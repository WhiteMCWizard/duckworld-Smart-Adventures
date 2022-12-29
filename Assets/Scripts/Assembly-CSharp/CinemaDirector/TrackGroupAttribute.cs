using System;
using System.Collections.Generic;

namespace CinemaDirector
{
	[AttributeUsage(AttributeTargets.Class)]
	public class TrackGroupAttribute : Attribute
	{
		private string label;

		private List<TimelineTrackGenre> trackGenres = new List<TimelineTrackGenre>();

		public string Label
		{
			get
			{
				return label;
			}
		}

		public TimelineTrackGenre[] AllowedTrackGenres
		{
			get
			{
				return trackGenres.ToArray();
			}
		}

		public TrackGroupAttribute(string label, params TimelineTrackGenre[] TrackGenres)
		{
			this.label = label;
			trackGenres.AddRange(TrackGenres);
		}
	}
}
