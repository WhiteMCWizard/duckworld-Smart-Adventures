using System;
using System.Collections.Generic;

namespace CinemaDirector
{
	[AttributeUsage(AttributeTargets.Class)]
	public class TimelineTrackAttribute : Attribute
	{
		private string label;

		private List<TimelineTrackGenre> trackGenres = new List<TimelineTrackGenre>();

		private List<CutsceneItemGenre> itemGenres = new List<CutsceneItemGenre>();

		public string Label
		{
			get
			{
				return label;
			}
		}

		public TimelineTrackGenre[] TrackGenres
		{
			get
			{
				return trackGenres.ToArray();
			}
		}

		public CutsceneItemGenre[] AllowedItemGenres
		{
			get
			{
				return itemGenres.ToArray();
			}
		}

		public TimelineTrackAttribute(string label, TimelineTrackGenre[] TrackGenres, params CutsceneItemGenre[] AllowedItemGenres)
		{
			this.label = label;
			trackGenres.AddRange(TrackGenres);
			itemGenres.AddRange(AllowedItemGenres);
		}

		public TimelineTrackAttribute(string label, TimelineTrackGenre TrackGenre, params CutsceneItemGenre[] AllowedItemGenres)
		{
			this.label = label;
			trackGenres.Add(TrackGenre);
			itemGenres.AddRange(AllowedItemGenres);
		}
	}
}
