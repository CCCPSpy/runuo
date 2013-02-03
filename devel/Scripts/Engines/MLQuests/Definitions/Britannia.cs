using System;
using System.Collections.Generic;
using Server;
using Server.ContextMenus;
using Server.Engines.MLQuests.Objectives;
using Server.Engines.MLQuests.Rewards;
using Server.Items;
using Server.Misc;
using Server.Mobiles;

namespace Server.Engines.MLQuests.Definitions
{
	#region Quests

	public class Aemaeth1 : MLQuest
	{
		public override Type NextQuest { get { return typeof( Aemaeth2 ); } }

		public Aemaeth1()
		{
			Activated = true;
			Title = 1075321; // Aemaeth
			Description = 1075322; // My father died in an accident some months ago. My mother refused to accept his death. We had a little money set by, and she took it to a necromancer, who promised to restore my father to life. Well, he revived my father, all right, the cheat! Now my father is a walking corpse, a travesty . . . a monster. My mother is beside herself -- she won't eat, she can't sleep. I prayed at the shrine of Spirituality for guidance, and I must have fallen asleep. When I awoke, there was this basin of clear water. I cannot leave my mother, for I fear what she might do to herself. Could you take this to the graveyard, and give it to what is left of my father?
			RefusalMessage = 1075324; // Oh! Alright then. I hope someone comes along soon who can help me, or I dont know what will become of us.
			InProgressMessage = 1075325; // My father - or what remains of him - can be found in the graveyard northwest of the city.
			CompletionMessage = 1075326; // What is this you give me? A basin of water?

			Objectives.Add( new DeliverObjective( typeof( BasinOfCrystalClearWater ), 1, 1075303, typeof( SkeletonOfSzandor ), "Szandor" ) );

			Rewards.Add( new DummyReward( 1075323 ) ); // Aurelia's gratitude.
		}

		public override bool CanOffer( BaseCreature quester, PlayerMobile pm )
		{
			if ( !base.CanOffer( quester, pm ) )
				return false;

			MLQuestContext context = MLQuestSystem.GetContext( pm );

			// This quest can be repeated, but only while the next quest has not been completed.
			if ( context != null && context.HasDoneQuest( NextQuest ) )
			{
				MLQuestSystem.Tell( quester, pm, 1075454 ); // I cannot offer you the quest again.
				return false;
			}

			return true;
		}

		public override void OnComplete( MLQuestInstance instance )
		{
			instance.Player.SendLocalizedMessage( 1046258, "", 0x23 ); // Your quest is complete.
		}

		public override void Generate()
		{
			base.Generate();

			PutSpawner( new Spawner( 1, 5, 10, 0, 4, "Aurelia" ), new Point3D( 1459, 3795, 0 ), Map.Trammel );
			PutSpawner( new Spawner( 1, 5, 10, 0, 4, "Aurelia" ), new Point3D( 1459, 3795, 0 ), Map.Felucca );
		}
	}

	public class Aemaeth2 : MLQuest
	{
		public override bool IsChainTriggered { get { return true; } }

		public Aemaeth2()
		{
			Activated = true;
			OneTimeOnly = true;
			Title = 1075327; // Aemaeth
			Description = 1075328; // You tell me it is time to leave this flesh. I did not understand until now. I thought: I can see my wife and my daughter, I can speak. Is this not life? But now, as I regard my reflection, I see what I have become. This only a mockery of life. Thank you for having the courage to show me the truth. For the love I bear my wife and daughter, I know now that I must pass beyond the veil. Will you return this basin to Aurelia? She will know by this that I am at rest.
			RefusalMessage = 1075330; // You wont take this back to my daughter? Please, I cannot leave until she knows I am at peace.
			InProgressMessage = 1075331; // My daughter will be at my home, on the east side of the city.
			CompletionMessage = 1075332; // Thank goodness! Now we can honor my father for the great man he was while he lived, rather than the horror he became.

			Objectives.Add( new DeliverObjective( typeof( BasinOfCrystalClearWater ), 1, 1075303, typeof( Aurelia ), "Aurelia" ) );

			Rewards.Add( new ItemReward( 1075304, typeof( MirrorOfPurification ) ) );
		}

		public override void OnComplete( MLQuestInstance instance )
		{
			instance.Player.SendLocalizedMessage( 1046258, "", 0x23 ); // Your quest is complete.
		}

		public override void Generate()
		{
			base.Generate();

			PutSpawner( new Spawner( 1, 5, 10, 0, 2, "SkeletonOfSzandor" ), new Point3D( 1277, 3731, 0 ), Map.Trammel );
			PutSpawner( new Spawner( 1, 5, 10, 0, 2, "SkeletonOfSzandor" ), new Point3D( 1277, 3731, 0 ), Map.Felucca );
		}
	}

	public class OddsAndEnds : MLQuest
	{
		public OddsAndEnds()
		{
			Activated = true;
			OneTimeOnly = true;
			Title = 1074354; // Odds and Ends
			Description = 1074677; // I've always been fascinated by primitive cultures -- especially the artifacts.  I'm a collector, you see.  I'm working on building my troglodyte display and I'm saddened to say that I'm short on examples of religion and superstition amongst the creatures.  If you come across any primitive fetishes, I'd be happy to trade you something interesting for them.
			RefusalMessage = 1072270; // Well, okay. But if you decide you are up for it after all, c'mon back and see me.
			InProgressMessage = 1074678; // I don't really want to know where you get the primitive fetishes, as I can't support the destruction of their lifestyle and culture. That would be wrong.
			CompletionMessage = 1074679; // Bravo!  These fetishes are just what I needed.  You've earned this reward.

			Objectives.Add( new CollectObjective( 12, typeof( PrimitiveFetish ), 1151986 ) ); // Primitive Fetishes

			Rewards.Add( ItemReward.BagOfTreasure );
		}
	}

	#endregion

	#region Mobiles

	public class Aurelia : BaseCreature
	{
		public override bool IsInvulnerable { get { return true; } }

		[Constructable]
		public Aurelia()
			: base( AIType.AI_Vendor, FightMode.None, 2, 1, 0.5, 2 )
		{
			Name = "Aurelia";
			Title = "the Architect's Daughter";
			Race = Race.Human;
			BodyValue = 0x191;
			Female = true;
			Hue = Race.RandomSkinHue();
			InitStats( 100, 100, 25 );

			Utility.AssignRandomHair( this, true );

			AddItem( new Backpack() );
			AddItem( new Sandals( Utility.RandomPinkHue() ) );

			if ( Utility.RandomBool() )
				AddItem( new Kilt( Utility.RandomPinkHue() ) );
			else
				AddItem( new Skirt( Utility.RandomPinkHue() ) );

			AddItem( new FancyShirt( Utility.RandomRedHue() ) );
		}

		public Aurelia( Serial serial )
			: base( serial )
		{
		}

		public override void Serialize( GenericWriter writer )
		{
			base.Serialize( writer );

			writer.Write( (int) 0 ); // version
		}

		public override void Deserialize( GenericReader reader )
		{
			base.Deserialize( reader );

			int version = reader.ReadInt();
		}
	}

	public class SkeletonOfSzandor : BaseCreature
	{
		public override bool IsInvulnerable { get { return true; } }

		[Constructable]
		public SkeletonOfSzandor()
			: base( AIType.AI_Vendor, FightMode.None, 2, 1, 0.5, 2 )
		{
			Name = "Skeleton of Szandor";
			Title = "the Late Architect";
			Hue = 0x83F2; // TODO: Random human hue? Why???
			Body = 0x32;
			InitStats( 100, 100, 25 );
		}

		public SkeletonOfSzandor( Serial serial )
			: base( serial )
		{
		}

		public override void Serialize( GenericWriter writer )
		{
			base.Serialize( writer );

			writer.Write( (int) 0 ); // version
		}

		public override void Deserialize( GenericReader reader )
		{
			base.Deserialize( reader );

			int version = reader.ReadInt();
		}
	}

	#endregion
}