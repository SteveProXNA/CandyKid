using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using WindowsGame.Data;
using WindowsGame.Objects;
using WindowsGame.Static;

namespace WindowsGame.Managers
{
	public interface IEntityManager
	{
		void Initialize();
		void LoadContent();
		void MoveEnemies(GameTime gameTime);
		void Move(BaseObject entity, Direction direction);

		void Draw();
		void DrawPlayer();
		void DrawEnemies();
		void DrawEnemy(EnemyType key);
		void ResetPlayer();
		void ResetEnemies();
		void ResetEnemy(EnemyType enemyType);
		void UpdatePlayerVelocity(Byte velocity);
		void UpdateEnemyVelocity(EnemyType enemyType, Byte velocity);
		void UpdateEnemyAttacker(EnemyType enemyType, Byte attacker);
		void UpdateMoveEnemies();
		CandyKid GetBasePlayer(Rectangle source, Byte location, Byte velocity);

		CandyKid Player { get; }
		IDictionary<EnemyType, CandyMama> Enemies { get; }
	}

	public class EntityManager : BaseManager, IEntityManager
	{
		private Boolean moveAdriana, moveSuzanne, moveStevePro;

		public void Initialize()
		{
			Player = GetBasePlayer(MyGame.Manager.ImageManager.GamerOneRectangle[BaseData.GamerSpriteIndex], BaseData.PlayerPosition, BaseData.PlayerVelocity);
			Player.Initialize(BaseData.MinTile, BaseData.MaxTile);

			Enemies = new Dictionary<EnemyType, CandyMama>(Constants.ENEMY_NUMBER)
			{
				{EnemyType.Adriana, GetBaseEnemy(MyGame.Manager.ImageManager.EnemyOneRectangle[BaseData.EnemyOneSpriteIndex], BaseData.AdrianaPosition, BaseData.AdrianaVelocity, BaseData.AdrianaAttacker)},
				{EnemyType.Suzanne, GetBaseEnemy(MyGame.Manager.ImageManager.EnemyTwoRectangle[BaseData.EnemyTwoSpriteIndex], BaseData.SuzannePosition, BaseData.SuzanneVelocity, BaseData.SuzanneAttacker)},
				{EnemyType.StevePro, GetBaseEnemy(MyGame.Manager.ImageManager.EnemyXyzRectangle[BaseData.EnemyXyzSpriteIndex], BaseData.SteveProPosition, BaseData.SteveProVelocity, BaseData.SteveProAttacker)},
			};

			Enemies[EnemyType.Adriana].Initialize(BehaveType.Vertical);
			Enemies[EnemyType.Suzanne].Initialize(BehaveType.Random);
			Enemies[EnemyType.StevePro].Initialize(BehaveType.Horizontal);
		}

		public void LoadContent()
		{
			Player.SetSource(MyGame.Manager.ImageManager.GamerOneRectangle[BaseData.GamerSpriteIndex]);
			Enemies[EnemyType.Adriana].SetSource(MyGame.Manager.ImageManager.EnemyOneRectangle[BaseData.EnemyOneSpriteIndex]);
			Enemies[EnemyType.Suzanne].SetSource(MyGame.Manager.ImageManager.EnemyTwoRectangle[BaseData.EnemyTwoSpriteIndex]);
			Enemies[EnemyType.StevePro].SetSource(MyGame.Manager.ImageManager.EnemyXyzRectangle[BaseData.EnemyXyzSpriteIndex]);

			UpdateMoveEnemies();
		}

		public void MoveEnemies(GameTime gameTime)
		{
			TileType[,] boardData = MyGame.Manager.BoardManager.BoardData;
			if (moveAdriana)
			{
				MoveEnemy(gameTime, boardData, EnemyType.Adriana);
			}
			if (moveSuzanne)
			{
				MoveEnemy(gameTime, boardData, EnemyType.Suzanne);
			}
			if (moveStevePro)
			{
				MoveEnemy(gameTime, boardData, EnemyType.StevePro);
			}
		}
		private void MoveEnemy(GameTime gameTime, TileType[,] boardData, EnemyType enemyType)
		{
			CandyMama enemyObject = Enemies[enemyType];
			if (Lifecycle.Dead != enemyObject.Lifecycle)
			{
				enemyObject.UpdateFrame(gameTime);
			}

			if (Direction.None == enemyObject.Direction && Lifecycle.Idle == enemyObject.Lifecycle)
			{
				IList<Direction> directionList = MyGame.Manager.MoveManager.CheckFreeMoves(boardData, (Byte)enemyObject.CurrX, (Byte)enemyObject.CurrY);
				Direction direction = MyGame.Manager.BotAIManager.GotoDirX(directionList, Player, enemyType, enemyObject);
				if (Direction.None != direction)
				{
					MyGame.Manager.EventManager.AddEnemyMoveEvent(enemyType, direction);
				}
			}
			else if (Direction.None != enemyObject.Direction && Lifecycle.Move == enemyObject.Lifecycle)
			{
				enemyObject.Update(gameTime);
			}
			if (Direction.None != enemyObject.Direction && Lifecycle.Idle == enemyObject.Lifecycle)
			{
				enemyObject.Stop();
			}
		}

		public void Move(BaseObject entity, Direction direction)
		{
			entity.Move(direction);
		}

		public void Draw()
		{
			DrawPlayer();
			DrawEnemies();
		}
		public void DrawPlayer()
		{
			Player.Draw();
		}
		public void DrawEnemies()
		{
			Enemies[EnemyType.StevePro].Draw();
			Enemies[EnemyType.Adriana].Draw();
			Enemies[EnemyType.Suzanne].Draw();
		}
		public void DrawEnemy(EnemyType key)
		{
			Enemies[key].Draw();
		}
		public void ResetPlayer()
		{
			Player.Reset();
		}
		public void ResetEnemies()
		{
			foreach (var key in Enemies.Keys)
			{
				ResetEnemy(key);
			}
		}
		public void ResetEnemy(EnemyType enemyType)
		{
			Enemies[enemyType].Reset();
		}

		public void UpdatePlayerVelocity(Byte velocity)
		{
			Player.SetVelocity(velocity);
		}
		public void UpdateEnemyVelocity(EnemyType enemyType, Byte velocity)
		{
			Enemies[enemyType].SetVelocity(velocity);
		}
		public void UpdateEnemyAttacker(EnemyType enemyType, Byte attacker)
		{
			Enemies[enemyType].SetAttacker(attacker);
		}

		public void UpdateMoveEnemies()
		{
			moveAdriana = BaseData.MoveAdriana;
			moveSuzanne = BaseData.MoveSuzanne;
			moveStevePro = BaseData.MoveStevePro;
		}

		public CandyKid GetBasePlayer(Rectangle source, Byte location, Byte velocity)
		{
			Byte row, col;
			MyGame.Manager.BoardManager.CalcPosition(location, out row, out col);

			Vector2 position = GetVector2(col, row);
			return new CandyKid(col, row, position, source, BaseData.EntityOffset, velocity, BaseData.TilesSize, BaseData.GamerSize);
		}

		public CandyKid Player { get; private set; }
		public IDictionary<EnemyType, CandyMama> Enemies { get; private set; }

		private static CandyMama GetBaseEnemy(Rectangle source, Byte location, Byte velocity, Byte attacker)
		{
			Byte row, col;
			MyGame.Manager.BoardManager.CalcPosition(location, out row, out col);

			Vector2 position = GetVector2(col, row);
			return new CandyMama(col, row, position, source, BaseData.EntityOffset, velocity, BaseData.TilesSize, BaseData.GamerSize, attacker);
		}
		private static Vector2 GetVector2(Byte x, Byte y)
		{
			Byte tilesSize = BaseData.TilesSize;
			Byte gameOffsetX = BaseData.GameOffsetX;
			Byte entityOffset = BaseData.EntityOffset;

			return new Vector2(x * tilesSize + tilesSize + gameOffsetX + entityOffset, y * tilesSize + tilesSize + entityOffset);
		}

	}
}