using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using WindowsGame.Data;
using WindowsGame.Objects;
using WindowsGame.Static;

namespace WindowsGame.Managers
{
	public interface INewArrowManager
	{
		void Initialize();
		void LoadContent();
		void Draw(Direction direction);

		void SetQuadrants(Byte option);
		IDictionary<Quadrant, NewArrow> QuadArrowDictionary { get; }
	}

	public class NewArrowManager : INewArrowManager
	{
		private IDictionary<Quadrant, Vector2>[] postionDictionary;
		private IList<NewArrow> newArrowList;

		private NewArrow upArrow, downArrow, leftArrow, rightArrow;
		private Rectangle normalVert, normalHorz, invertVert, invertHorz;

		public void Initialize()
		{
			QuadArrowDictionary = new Dictionary<Quadrant, NewArrow>(Constants.ARROW_NUMBER);
		}

		public void LoadContent()
		{
			postionDictionary = GetPostionDictionary();

			normalVert = new Rectangle(0 * BaseData.ArrowSize, 0 * BaseData.ArrowSize, BaseData.ArrowSize, BaseData.ArrowSize);
			normalHorz = new Rectangle(1 * BaseData.ArrowSize, 0 * BaseData.ArrowSize, BaseData.ArrowSize, BaseData.ArrowSize);
			invertVert = new Rectangle(0 * BaseData.ArrowSize, 1 * BaseData.ArrowSize, BaseData.ArrowSize, BaseData.ArrowSize);
			invertHorz = new Rectangle(1 * BaseData.ArrowSize, 1 * BaseData.ArrowSize, BaseData.ArrowSize, BaseData.ArrowSize);

			upArrow = new NewArrow(Direction.Up, normalVert, invertVert, SpriteEffects.None);
			downArrow = new NewArrow(Direction.Down, normalVert, invertVert, SpriteEffects.FlipVertically);
			leftArrow = new NewArrow(Direction.Left, normalHorz, invertHorz, SpriteEffects.None);
			rightArrow = new NewArrow(Direction.Right, normalHorz, invertHorz, SpriteEffects.FlipHorizontally);

			newArrowList = new List<NewArrow>();
			newArrowList.Add(upArrow);
			newArrowList.Add(downArrow);
			newArrowList.Add(leftArrow);
			newArrowList.Add(rightArrow);

			SetQuadrants(BaseData.NewArrowIndex);
		}

		private static IDictionary<Quadrant, Vector2>[] GetPostionDictionary()
		{
			Byte gameOffsetX = BaseData.GameOffsetX;
			Byte tilesSize = BaseData.TilesSize;
			Byte candySize = BaseData.CandySize;
			Byte tileRatio = BaseData.TileRatio;

			var dictionary0 = new Dictionary<Quadrant, Vector2>(Constants.ARROW_NUMBER)
			{
				{Quadrant.TopLeft, new Vector2(0 * gameOffsetX + 0 * tilesSize + tileRatio, 2 * tilesSize + candySize)},
				{Quadrant.BotLeft, new Vector2(0 * gameOffsetX + 0 * tilesSize + tileRatio, tileRatio * tilesSize + 0)},
				{Quadrant.TopRight, new Vector2(1 * gameOffsetX + candySize * tilesSize + tileRatio, 2 * tilesSize + candySize)},
				{Quadrant.BotRight, new Vector2(1 * gameOffsetX + candySize * tilesSize + tileRatio, tileRatio * tilesSize + 0)}
			};

			var dictionary1 = new Dictionary<Quadrant, Vector2>(Constants.ARROW_NUMBER)
			{
				{Quadrant.TopLeft, new Vector2((240 - 64) / 2 + 560, 16 + 160)},
				{Quadrant.BotLeft, new Vector2((240 - 64) / 2 + 560, 400)},
				{Quadrant.TopRight, new Vector2(8 + 560, 128 + 160)},
				{Quadrant.BotRight, new Vector2(240 - 64 - 8 + 560, 128 + 160)}
			};

			var dictionary = new Dictionary<Quadrant, Vector2>[Constants.MAX_LAYOUT];
			dictionary[(Byte)LayoutType.Custom] = dictionary0;
			dictionary[(Byte)LayoutType.BotRight] = dictionary1;

			return dictionary;
		}
		public void Draw(Direction direction)
		{
			foreach (var arrow in QuadArrowDictionary.Values)
			{
				if (direction == arrow.Direction)
				{
					arrow.DrawInvert();
				}
				else
				{
					arrow.DrawNormal();
				}
			}
			
		}

		public void SetQuadrants(Byte option)
		{
			switch (option)
			{
				// Custom
				case 0:
					SetQuadrants(Direction.Up, Direction.Down, Direction.Left, Direction.Right);
					break;

				// Up.
				case 1:
					SetQuadrants(Direction.Up, Direction.Down, Direction.Left, Direction.Right);
					break;
				case 2:
					SetQuadrants(Direction.Up, Direction.Left, Direction.Down, Direction.Right);
					break;

				// Left.
				case 3:
					SetQuadrants(Direction.Left, Direction.Right, Direction.Up, Direction.Down);
					break;
				case 4:
					SetQuadrants(Direction.Left, Direction.Up, Direction.Right, Direction.Down);
					break;

				default:
					break;
			}
		}
		private void SetQuadrants(Direction topLeft, Direction botLeft, Direction topRight, Direction botRight)
		{
			SetQuadrant(Quadrant.TopLeft, ConvertDirectionToByte(topLeft));
			SetQuadrant(Quadrant.BotLeft, ConvertDirectionToByte(botLeft));
			SetQuadrant(Quadrant.TopRight, ConvertDirectionToByte(topRight));
			SetQuadrant(Quadrant.BotRight, ConvertDirectionToByte(botRight));
		}
		private void SetQuadrant(Quadrant quad, Byte item)
		{
			NewArrow arrow = newArrowList[item];
			var tmpDictionary = postionDictionary[(Byte)BaseData.GameLayout];
			Vector2 position = tmpDictionary[quad];

			arrow.SetPosition(position);
			QuadArrowDictionary[quad] = arrow;
		}
		private Byte ConvertDirectionToByte(Direction direction)
		{
			if (Direction.Up == direction)		{ return 0; }
			if (Direction.Down == direction)	{ return 1; }
			if (Direction.Left == direction)	{ return 2; }
			if (Direction.Right == direction)	{ return 3; }
			return 0;
		}

		public IDictionary<Quadrant, NewArrow> QuadArrowDictionary { get; private set; }
	}
}