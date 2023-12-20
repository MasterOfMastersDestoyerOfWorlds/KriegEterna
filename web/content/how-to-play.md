---
title: "How to Play"
description: 'Krieg Eterna How To Play'
layout: single
---
<section class="gradient even-gradient">
<div class="css-pjOS5 css-P-g66 css-EiWA- css-ysAew">
<div class="title-wrapper">
    <h3>How To Play</h2>
</div>
    {{< vid  "https://www.youtube.com/embed/zam0XP3T0UM?si=GVfCjiTFEX7I50vP">}}
    <br />
    <a href="/assets/Rules.pdf" download>
        Rulebook Download
    </a>
    <br>
    <p class="rule-paragraph">
        Krieg Eterna is a game of strategizing and bluffing your way to victory across a series of
        battles against your opponent. Command your army in the field to attack with precision,
        deceive enemy officers, and overrun opposing regiments!
    </p>
    <h3>Gameplay</h3>
    <p class="rule-paragraph">
        A game of Krieg Eterna is divided into three separate rounds of play, where your ultimate
        objective is to win more rounds than your opponent. Players take turns playing units and
        powers to build the strength of their army. At the end of each round, the player with the
        most strength across all of their rows wins that round.
    </p>
    <picture>
        <source srcset="/images/LayoutRender.png?fm=webp" type="image/webp"> <img
            src="/images/DeluxeDeckRender.png" alt="" width="548" height="433" loading="lazy"
            class="css-u6Ex featured-image">
    </picture>
    <p class="rule-paragraph">
        At the start of the game, each player draws 9 unit cards, 4 power cards, and 1 King card.
        After review, players must discard 3 of their cards (any type) before play begins. Unless a
        card says otherwise, these are the only cards drawn for the entire game. Players then take
        turns playing one card at a time until all players have said "pass", after which they score
        the field, discard all cards on the field to the graveyards (face up), and start the next
        round.
    </p>
    <div class="title-wrapper">
    <h3>Playing Field</h3>
    </div>
    <p class="rule-paragraph">
        Next is a diagram showing the layout of the playing field. Each player's side of
        the field is split into three rows: Melee, Range, and Siege. There is no limit to the number
        of cards played in rows, and unit cards may be rearranged within a row at any time.
    </p>
    <picture>
        <source srcset="/images/PlayingField.png?fm=webp" type="image/webp"> <img
            src="/images/PlayingField.png" alt="" width="548" height="433" loading="lazy"
            class="css-u6Ex featured-image">
    </picture>
    <h3>Cards</h3>
    <p class="rule-paragraph">
        There are three separate decks of cards shared between all players: the unit deck, power
        deck, and King deck -- each with a different colored card-backing. Each deck also has its
        own graveyard, in which discarded cards are placed face up. Within each deck there are
        different card types, indicated by the symbol in the upper-right corner of a card.
    </p>
    {{< card-scroll-gallery-rules >}}
        {{< cardflip front="Flag" back="Crusader" >}}
        {{< cardflip front="FlagPower" back="Shipwreck" >}}
        {{< cardflip front="FlagKing" back="Terror" >}}
    {{</ card-scroll-gallery-rules >}}
    <p class="" style="text-align: center; margin-bottom: 15px;">
        Click the cards to flip them over!
    </p>
    <h3>Units</h3>
    <p class="rule-paragraph">
        The unit deck consists of Melee, Ranged, and Siege cards. All unit cards have a base
        strength number in the upper-right corner, indicating how many points they will score at the
        end of a round. The total strength of a unit can be increased or decreased beyond the base
        strength using the effects of other cards.
    </p>
    {{< card-scroll-gallery-rules >}}
        {{< cardflip front="Landsknecht" back="Flag" >}}
        {{< cardflip front="Grenadier" back="Flag" >}}
        {{< cardflip front="Cannon3" back="Flag" >}}
    {{</ card-scroll-gallery-rules >}}
    {{< icon-paragraph icon="Melee" text="Melee units go in the front row, closest to your opponent." >}}
    {{< icon-paragraph icon="Ranged" text="Ranged units go in the middle row." >}}
    {{< icon-paragraph icon="Siege" text="Siege units go in the back row, closest to you." >}}
    <h3>Powers</h3>
    <p class="rule-paragraph">
        The power deck consists of Weather, Spy, Jester, and Hex cards, each with a card effect
        granting unique abilities. Unless otherwise indicated, these cards are played to the side
        (not in the Melee, Range, or Siege rows) and immediately go to the graveyard after use.
    </p>
    {{< card-scroll-gallery-rules >}}
        {{< cardflip front="Frost" back="FlagPower" >}}
        {{< cardflip front="Saboteur" back="FlagPower" >}}
        {{< cardflip front="Retreat" back="FlagPower" >}}
        {{< cardflip front="Styx" back="FlagPower" >}}
    {{</ card-scroll-gallery-rules >}}
    {{< icon-paragraph icon="Weather" text="Weather cards affect the total strength of one of the rows, for all players. Within the affected row, Weather applies individually to each unit; always round unit strength down when dividing, with a minimum strength of one per unit. Keep Weather cards on the field until their effects end." >}}
    <img src="/gif/WeatherEx.gif" alt="" class="landscape-gif" unselectable="on" loading="lazy">
    {{< icon-paragraph icon="Spy" text="Spy cards allow you to draw cards, but are played as units on your opponent's side of the field, thereby adding to your opponent's total strength. At the end of the round, most Spys go directly to the hand of the player whose side of the field they are on, allowing your opponent to turn your Spys against you." >}}
    {{< icon-paragraph icon="Jester" text="Jester cards can return cards on the field back to your hand, or set them aside for the next round. Use them to deceive your opponent." >}}
    {{< icon-paragraph icon="Power" text="Hex cards allow for drawing cards, sending cards to the graveyard, or other useful effects. Hex cards attached to a specific unit follow with that unit, e.g. to the graveyard, another row, or even a player's hand." >}}
    <h3>Kings</h3>
    <p class="rule-paragraph">
        The King deck contains all King cards, and each player should choose one at random at the
        start of the game. Each player's King card can only be scored in one round per game (i.e. it
        does not return to your hand each round).King cards are not affected by other cards, unless
        specifically mentioned.
    </p>
    {{< cardflip front="Lion" back="FlagKing" >}}
    {{< icon-paragraph icon="King" text="King cards allow you to double the strength of one of your rows, as well as having a choice between other unique effects. A King only doubles the row while it remains on the field." >}}
    <h3>Adjacency</h3>
    <p class="rule-paragraph">
        If two units with the same strength are in the same row, they get an adjacency bonus which
        doubles the strength of both units. Adjacency bonuses only apply to units with two or three
        strength. You cannot add a third unit to this adjacent pair, but you can have multiple pairs
        on the field. Note that adjacency effects can be enabled or broken by power cards that
        modify a unit's strength.
    </p>
    <p class="rule-paragraph">
        Example 1: A row has 3 units with base strength 2. Two units form an adjacent pair, but the
        third does not get a bonus. The row's total score is: (2 × 2) + (2 × 2) + 2 = 10.
    </p>
    <img src="/gif/AdjacencyEx1.gif" alt="" class="landscape-gif" unselectable="on" loading="lazy">
    <p class="rule-paragraph">
        Example 2: A row has 2 units with base strength 4. A certain power is then played to
        subtract 1 strength from all units in that row. Both units now have strength 3, and so
        receive an adjacency bonus. The row's total score is: ((4 - 1) × 2) + ((4 - 1) × 2) = 12.
    </p>
    <img src="/gif/AdjacencyEx2.gif" alt="" class="landscape-gif" unselectable="on" loading="lazy">
    <h3>Passing</h3>
    <p class="rule-paragraph">
        Before playing any cards on your turn, you may say \'pass\' to signal you are ready to end
        the round. Once a player has passed, they may not play any more cards until the next round.
        Other players may continue to play cards, taking as many turns as desired before eventually
        passing themselves. After all players have passed, the round is scored.
    </p>
    <div  class="title-wrapper">
    <h3>Scoring The Round</h3>
    </div>
    <p class="rule-paragraph">
        Each of your units on the field contributes its total strength to your score for the round
        as follows:
    <p class="rule-paragraph">
        1. Start with each unit's base strength number found in the upper-right corners of the
        cards.
    </p>
    <p class="rule-paragraph">
        2. Apply all modifiers with keywords: set base strength.
    </p>
    <p class="rule-paragraph">
        3. Apply all modifiers with keywords: add or subtract base strength.
    </p>
    <p class="rule-paragraph">
        4. If there are adjacency bonuses at this point, double the strength of units forming
        adjacent pairs.
    </p>
    <p class="rule-paragraph">
        5. Apply all row multipliers (keywords: double or halve strength) from weather effects and
        King bonuses to each unit. Always round down if dividing, with a minimum strength of one per
        unit.
    </p>
    </p>
    <h3>Winning</h3>
    <p class="rule-paragraph">
        After adding all units' total strengths together, the player with the highest number wins
        the round (the size of the victory does not matter).
    </p>
    <p class="rule-paragraph">
        The first player to win two rounds wins the game. If no player has won two rounds and no
        player can play any cards, the game is a draw.
    </p>
    <div class="title-wrapper">
    <h3>Final Details</h3>
    </div>
    <p class="rule-paragraph">
        Going first: In the first round, the player who dealt the unit deck (or won the last game)
        goes first. In later rounds, the previous round's winner goes first.
    </p>
    <p class="rule-paragraph">
        If a card says to draw, you may pick any combination of unit or power cards, unless
        otherwise specified. King cards are never drawn after the initial setup.
    </p>
    <p class="rule-paragraph">
        If an opponent asks, you must tell them the number of unit and power cards you have in your
        hand.
    </p>
    <p class="rule-paragraph">
        Set aside cards do not score for the current round and cannot be affected by other cards
        this round. At the start of the next round, return them directly to the field, in their same
        row (do this before the first turn is taken). Do not repeat any card effects.
    </p>
    <p class="rule-paragraph">
        If a power card has an activation cost (e.g. "send a unit to the graveyard"), you must pay
        its price in order to play the card at all. This prevents players from stalling with
        unusable/undesired power cards.
    </p>
    <div  class="title-wrapper">
    <h3>Additional Players</h3>
    </div>
    <p class="rule-paragraph">
        When playing Krieg Eterna with more than two players, you can choose to either play in teams
        or a free-for-all:
    </p>
    <p class="rule-paragraph">
        Players should not show their hands to teammates.
    </p>
    <p class="rule-paragraph">
        Each player has their own separate set of rows (e.g. teammates do not share rows, adjacency,
        or Kings).
    </p>
    <p class="rule-paragraph">
        Turn order should alternate between teams.
    </p>
    <p class="rule-paragraph">
        Teammates can never play on your turn for you, even if you have already passed.
    </p>
    <p class="rule-paragraph">
        Scoring: In free-for-all, award points based on a player's ranking within each round. In a
        team game, the team with the highest combined total strength wins the round.
    </p>
</div>
</section>