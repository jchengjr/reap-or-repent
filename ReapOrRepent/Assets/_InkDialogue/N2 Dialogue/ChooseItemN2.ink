// Night 2 item choice: after leaving the house, the Reaper picks one tool to keep for the rest of the game.
// Reached from the Will knot in MN2.ink. The lines are placeholders for Davin to rewrite.

=== ChooseItemN2 ===

As you step off the porch, a second scroll from God unfurls in your hand.

<i>Choose one tool to carry with you for the nights ahead.</i>

    +[The lie detector.]
        ~ GiveItem("lie_detector")
    +[The skeleton key.]
        ~ GiveItem("skeleton_key")
    +[The washcloth.]
        ~ GiveItem("washcloth")

-
{
    - HasItem("lie_detector"):
        The lie detector hums faintly as you tuck it into your robes.
    - HasItem("skeleton_key"):
        The skeleton key is cold, and heavier than it looks.
    - HasItem("washcloth"):
        The washcloth is damp, somehow.
}

-> END
