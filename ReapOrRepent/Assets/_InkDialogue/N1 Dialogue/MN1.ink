VAR hasAskedNight = false
VAR hasAskedAnyone = false

=== startMN1 ===

...hello?

What um…brings you here tonight?
    +[<i>Show the scroll</i>]
        -> Scroll
        
=== Scroll ===

...

...I see.

Do you have a <b>name?</b>
    +[Reaper.]
        ->Reaper

=== Reaper ===

...I suspected so.

I'm <b>Mildred Crowell.</b>

-> mainNM1

=== mainNM1 ===

    +[How has your night been?]
        ->Night
    +[Is there anyone else?]
        ->Anyone
    +{hasAskedNight and hasAskedAnyone}[May I come in?]
        -> In

=== Night ===

...fine.

Just trying to do some cleaning.

~ hasAskedNight = true
-> mainNM1

=== Anyone ===

...no. Just me in there.

Well, there is my husband, <b>Hank.</b>

He uh, works 12 hour shifts at the <b>police department,</b> so I doubt he’ll be here while you’re around.

I hope that’s…not a problem.

~ hasAskedAnyone = true
->  mainNM1


=== In ===

Oh…right <i>now??</i>

Um…well it’s awfully <b>filthy.</b> I wouldn’t want to give a bad first impression.

Especially when…well…y’know…

I’ll let you in <b>tomorrow night</b> though, and I won’t be in your way.

I hope…that’s okay…

    +[I'll see you tomorrow]
        -> Tomorrow

=== Tomorrow ===

Thank you.

I’ll um…see you tomorrow too.

-> END



