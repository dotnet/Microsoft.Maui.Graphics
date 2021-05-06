using System;

// copied from: Microsoft.Maui/Core/src/Primitives/LineBreakMode.cs
// merged with: Xwt.Drawing/TextLayout.cs TextTrimming

namespace Microsoft.Maui.Graphics.Extras {

	[Flags]
	public enum LineBreakFlags {

		None = 0,

		Wrap = 0x1 << 0,
		Truncation = 0x1 << 1,
		Elipsis = 0x1 << 2,

		Character = 0x1 << 3,
		Word = 0x1 << 4,

		Start = 0x1 << 5,
		Center = 0x1 << 6,
		End = 0x1 << 7,

	}

	[Flags]
	public enum LineBreakMode {

		NoWrap = LineBreakFlags.None,

		WordWrap = LineBreakFlags.Wrap | LineBreakFlags.Word | LineBreakFlags.End,
		CharacterWrap = LineBreakFlags.Wrap | LineBreakFlags.Character | LineBreakFlags.End,
		WordCharacterWrap = LineBreakFlags.Wrap | LineBreakFlags.Word | LineBreakFlags.Character | LineBreakFlags.End,

		StartTruncation = LineBreakFlags.Truncation | LineBreakFlags.Character | LineBreakFlags.Start,
		EndTruncation = LineBreakFlags.Truncation | LineBreakFlags.Character | LineBreakFlags.End,
		CenterTruncation = LineBreakFlags.Truncation | LineBreakFlags.Character | LineBreakFlags.Center,

		HeadTruncation = StartTruncation,
		TailTruncation = EndTruncation,
		MiddleTruncation = CenterTruncation,

		WordElipsis = LineBreakFlags.Elipsis | LineBreakFlags.Word | LineBreakFlags.End,
		CharacterElipsis = LineBreakFlags.Elipsis | LineBreakFlags.Character | LineBreakFlags.End,

	}

}
