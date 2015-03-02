﻿namespace Cricket.CricketEngine.UnitTests

open FsUnit
open NUnit.Framework

open Cricket.CricketEngine

module SampleData =

    let sampleInnings =
        {
            Score = 50;
            BallsFaced = 80;
            HowOut = None;
            Fours = 5;
            Sixes = 1;
        }

    let sampleBowler =
        Name "testBowler"

    let sampleFielder =
        Name "testFielder"

    let sampleData = sampleInnings, sampleBowler, sampleFielder

[<TestFixture>]
type IndividualInningsScoreTests ()=

    let innings, bowler, _ = SampleData.sampleData

    static member TestData =
        [|
            DotBall, 0;
            ScoreRuns 1, 1;
            Four, 4;
            ScoreRuns 4, 4;
            Six, 6;
            Bowled, 0;
            LBW, 0;
            HitWicket, 0;
            Caught (SampleData.sampleFielder, false), 0;
            Stumped SampleData.sampleFielder, 0;
            RunOut (0, false), 0;
            RunOut (1, true), 1;
        |]

    [<TestCaseSource("TestData")>]
    member _x.``batsman's score increases correctly`` testData =
        let ball, scoreIncreasesBy = testData
        (Update bowler innings ball).Score |> should equal (innings.Score + scoreIncreasesBy)

[<TestFixture>]
type IndividualInningsHowOutTests ()=

    let innings, bowler, _ = SampleData.sampleData

    static member TestData =
        [|
            DotBall, None;
            ScoreRuns 1, None;
            Four, None;
            ScoreRuns 4, None;
            Six, None;
            Bowled, Some (HowOut.Bowled SampleData.sampleBowler);
            LBW, Some (HowOut.LBW SampleData.sampleBowler);
            HitWicket, Some (HowOut.HitWicket SampleData.sampleBowler);
            Caught (SampleData.sampleFielder, false), Some (HowOut.Caught (SampleData.sampleBowler, SampleData.sampleFielder));
            Caught (SampleData.sampleFielder, true), Some (HowOut.Caught (SampleData.sampleBowler, SampleData.sampleFielder));
            Stumped SampleData.sampleFielder, Some (HowOut.Stumped (SampleData.sampleBowler, SampleData.sampleFielder));
            RunOut (0, false), Some (HowOut.RunOut);
            RunOut (1, true), Some (HowOut.RunOut);
       |]

    [<TestCaseSource("TestData")>]
    member _x.``batsman is out correctly`` (testData: BallOutcome * HowOut option) =
        let ball, howOut = testData
        (Update bowler innings ball).HowOut |> should equal howOut

[<TestFixture>]
type IndividualInningsBallsFacedTests ()=

    let innings, bowler, _ = SampleData.sampleData

    static member TestData =
        [|
            DotBall, true;
            ScoreRuns 1, true;
            Four, true;
            ScoreRuns 4, true;
            Six, true;
            Bowled, true;
            LBW, true;
            HitWicket, true;
            Caught (SampleData.sampleFielder, false), true;
            Stumped SampleData.sampleFielder, true;
            RunOut (0, true), true;
            RunOut (1, false), true;
        |]

    [<TestCaseSource("TestData")>]
    member _x.``batsman's balls faced update correctly`` testData =
        let ball, ballsFacedIncreased = testData
        (Update bowler innings ball).BallsFaced |> should equal (innings.BallsFaced + if ballsFacedIncreased then 1 else 0)

[<TestFixture>]
type IndividualInningsFoursTests ()=

    let innings, bowler, _ = SampleData.sampleData

    static member TestData =
        [|
            DotBall, false;
            ScoreRuns 1, false;
            Four, true;
            ScoreRuns 4, true;
            Six, false;
            Bowled, false;
            LBW, false;
            HitWicket, false;
            Caught (SampleData.sampleFielder, false), false;
            Stumped SampleData.sampleFielder, false;
            RunOut (3, true), false;
            RunOut (3, false), false;
            RunOut (4, true), true;
            RunOut (4, false), true;
        |]

    [<TestCaseSource("TestData")>]
    member _x.``batsman's fours update correctly`` testData =
        let ball, foursIncreased = testData
        (Update bowler innings ball).Fours |> should equal (innings.Fours + if foursIncreased then 1 else 0)

[<TestFixture>]
type IndividualInningsSixesTests ()=

    let innings, bowler, _ = SampleData.sampleData

    static member TestData =
        [|
            DotBall, false;
            ScoreRuns 1, false;
            Four, false;
            Six, true;
            ScoreRuns 6, true;
            Bowled, false;
            LBW, false;
            HitWicket, false;
            Caught (SampleData.sampleFielder, false), false;
            Stumped SampleData.sampleFielder, false;
            RunOut (5, true), false;
            RunOut (5, false), false;
            RunOut (6, true), true;
            RunOut (6, false), true;
        |]

    [<TestCaseSource("TestData")>]
    member _x.``batsman's sixes update correctly`` testData =
        let ball, sixesIncreased = testData
        (Update bowler innings ball).Sixes |> should equal (innings.Sixes + if sixesIncreased then 1 else 0)