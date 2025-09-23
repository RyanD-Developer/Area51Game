using Godot;
using System;

namespace Game.Core
{
	public partial class Globals : Node
	{
		public static Globals Instance { get; private set; }

		[ExportCategory("Gameplay")]
		[Export] public int GlobalIntTest = 32;

		public override void _Ready()
		{
			Instance = this;

			Logger.Info("Loading Globals ...");
		}
	}
}
