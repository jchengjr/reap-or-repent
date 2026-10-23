=== MN2 === ///upon starting night 2, this is the knot that should play

Oh, hello.

...you’re very punctual. Midnight on the dot, same as yesterday.

    +[May I come in?]
        -> In
    +[I require entrance tonight.]
        -> In

=== In ===
...

You may, but I ask you stay away from some rooms.

    +[This will impede me.]
        -> Impede
    +[It will be your loss.]
        -> Impede

=== Impede ===

I-I know, and I’m sure that doesn’t look the best for us, but it’s…just…

I didn’t have time to clean the upstairs or our laundry room, and I just didn’t want you to see any mess is all.

So please…just for tonight.

    +[Understood.]
        -> Understood

=== Understood ===

Th-thank you.

    -> END

=== MildredHouseEnterN2 === ///plays after entering the house

If you uh…need to ask anything, I’ll just be right here.

-> END


=== MildredInHouseN2 === ///plays when you interact with Mildred in the house Night 2

Did you um…need something?

-> MN2Choices

=== MN2Choices ===

    +[Tell me about your husband.]
        -> Husband
    +[What is your daily routine?]
        -> Routine
    +[What are those couch stains?]
        -> Stains
    +[Who is the child?]
        -> Child
    +[I noticed medication.]
        -> Meds
    +[There's blood in your sink.]
        -> Sink
    +[Are you certain I cannot investigate more?]
        -> Investigate
    +[I'll see you tomorrow.]
        -> EndNight
    +[I'll be back.]
        -> END

=== Husband ===

Oh Hank? Well like I said he’s uh, not here.

He’s the police chief so he has long nights at the office.

It gets…lonely, when he’s not around.

The house has felt empty for quite some time, and his frequent absences certainly don’t help.

B-but it’s not too bad…all things considered.

-> MN2Choices

=== Routine ===

Um, nothing much? Certainly not what it used to be.

It’s mostly just housekeeping, at least as much as I can muster.

I cook, I clean, I look for jobs. Truly the life of someone in their 50s.

Hope that…suffices.

-> MN2Choices

=== Stains === //has seen stains

What stai-oh! I’m sorry, I thought I had cleaned those up.

It’s just some water spill from earlier. C-clumsy me.

//if has tissues true, give dialogue option. if nothing, return to choices.

T-tissues?

I-

...

Alright, yes, I was crying. You happy?

What’s so abnormal about a grieving mother crying now?! I-

...I’m sorry I…I don’t know what came over me, I just…

...it’s a touchy subject is all. You understand.

-> MN2Choices

=== Child ===

...

That’s my kid. Sylas.

He um…

Died in an accident a while back.

No, I don’t want to talk about it. No, I don’t want your sympathies.

I’ve already gone through that part of the process. I don’t need to do it again.

-> MN2ChildChoices

=== MN2ChildChoices ===

    +[Can I ask what he was like?]
        -> Like
    +[Could you tell me how old he is?]
        -> Old
    +[He sounds like a good kid.] //displays when previous two choices are selected.
        -> Good

=== Like ===

...he was the sweetest boy to ever exist.

He’d always go on and on about some dinosaur he saw in a book.

He always asked me why there aren’t any more around, and then Hank would say that they left because they thought people were too stinky.

I also remember when he’d-

When he...

...sorry. I just…I just can’t.

-> MN2ChildChoices

=== Old ===

Four.

He would’ve graduated preschool next year.

Now instead of seeing him walk across a stage, I saw him get ca-

...carried off to his grave.

It just…it isn’t…fair…

No casket deserves to be that small.

-> MN2ChildChoices

=== Good ===

Yeah…the best…

-> MN2Choices

=== Meds ===
“You-you dug through our bathroom cabinets?”
Th-that is a blatant breach of privacy!”
“I…I know that, I just thought you’d, I don’t know, have some basic decency or something!”
—
“What?? That doesn’t mean…just because you…”
“Look, I know investigating is your job, I just assumed you’d tell me when looking into more…private areas.”
“Either way, the medications I take are none of your business.”

-> MN2Choices

=== Sink ===
blood

-> MN2Choices
=== Investigate ===

ooo i saw 1 of every place why cant i go up bro

-> MN2Choices

=== EndNight ===

i got above dialogue and i wanna leave now

-> END //this should trigger end of game

//hasSeenStains
//hasSeenTissues
//hasSeenPortrait
//hasSeenNews
//hasSeenMeds
//hasSeenBlood
//hasSeenYoga
//hasSeenBathroom
//hasSeenLivingRoom
//hasSeenKitchen