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
    +[Why did you turn the TV off?]
        -> TV
    +[I noticed medication.]
        -> Meds
    +[There's blood in your sink.]
        -> Sink
    +[You have an overdue book.]
        -> Book
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

=== TV ===

S-sorry I just uh, don’t want to waste any more electricity.

With me being unemployed and all, we’re trying to save as much money as we can.

I hope you understand.

    +[What was the news talking about?]
        -> News
    +[Leave]
        -> END

=== News ===

Hm? I uh, honestly have no idea.

By the time Hank gets home he’s too tired to tell me about all his work stories, and I barely watch much TV anyways so…

I guess I’m a little out of the loop.

-> MN2Choices

=== Meds ===
You-you dug through our bathroom cabinets?

Th-that is a blatant <b>breach of privacy!</b>

    +[It's my job.]
        -> Job
    +[You knew this would happen.]
        -> Happen
        
=== Job ===

I…I know that, I just thought you’d, I don’t know, have some basic decency or something!

-> Private
=== Happen ===

What?? That doesn’t mean…just because you…

-> Private

=== Private ===

Look, I know investigating is your job, I just assumed you’d tell me when looking into more…<b>private</b> areas.

Either way, the <b>medications</b> I take are none of your business.

-> MN2Choices

=== Sink ===

W-what? There’s blood in our sink??

I have <b>no idea</b> where that could’ve came from I…

Maybe it’s from <b>Hank</b> after washing up after a shift? Or something?

I can um, ask him once he comes back and let you know about it <b>tomorrow?</b> If that’s ok?

-> MN2Choices

=== Book ===

“Hm? Oh, you must be talking about that yoga book.

“Yeah I just…haven’t really felt like leaving the house, and whenever I do, I forget to bring it.

“That thing’s been collecting dust for ages. Hopefully they give me some grace…

    +[Why yoga?]
        -> Yoga
    +[Leave]
        -> END

=== Yoga ===

I used to be a yoga instructor actually. Was pretty therapeutic.

Might…get back into that soon. Hopefully.

-> MN2Choices

=== Investigate ===

Well like I said, it’s awfully <b>filthy.</b>

I already feel bad with the current state of…well everything in this house.

I know how um…thorough you can be with your work, I just didn’t want to leave a bad first impression is all.

I’ll clean up more with Hank <b>tomorrow,</b> and uh, hopefully things should be more ready for your arrival.

-> MN2Choices

=== EndNight ===

Leaving so soon? Uh, alright!

I hope that we’ve made a good first impression.

We’ll um, see you <b>tomorrow</b> night?

    +[That you will]
        -> Will
        
=== Will ===

...great. See you then.


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