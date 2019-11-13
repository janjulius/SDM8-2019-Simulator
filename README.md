# SDM8-2019-Simulator
Simulator for software engineering class NHL Stenden hogeschool 2019

# Voorbeelden en gebruik

----
## Structuur Stoplight (voorbeeld):

### Hoofd object (communicatie) laagste laag:

#### SdmClient.cs

Behoud de connectie van het mqtt protocol, deze weet ook waarmee hij de connectie moet maken, alles wat over de mqtt moet sturen (of ontvangen) moet erven van dit object.
In veel gevallen bestaat er een beter object om van te erven, bijvoorbeeld een stoplight heeft deze structuur:

StopLight -> TrafficObject -> SdmClient

SdmClient connect automatisch met de gespecificeerde variablen in `Constants.cs`

SdmClient heeft een aantal handige overrideable methoden zoals:


SetUp, Refresh en ConnectedRefresh.

#### TrafficObject.cs

Dit object is een tussenstap en behoud dingen als de status (payload) en callt de `SetUp()` methode van de `SdmClient.cs` zodat je dat zelf niet hoeft te doen (mits deze niet geoverride word), ook is er een event genaamd `StatusUpdatedEvent` die wordt aangeroepen bij een verandering van de status.

#### StopLight.cs

Het stoplight doet dan het echte werk

    public override void SetUp()
    {
        base.SetUp();
        SetStatus(0);
        Subscribe(); 
    }

Door `Subscribe()` of `Publish()` aan te roepen zal dit object die specifieke SdmClient methode aanroepen, daarna override ik de `SetStatus(int)` methode zodat deze de benodigde methoden aanroept om het stoplight van kleur te laten veranderen.

    public override void SetStatus(int status)
    {
        base.SetStatus(status);
        UpdateStopLight();
    }

Een publish ziet er bijvoorbeeld zo uit:

    class Sensor : TrafficObject
    {
        private int previousStatus = 0;

        public override void ConnectedRefresh()
        {
            base.ConnectedRefresh();
        }

        public override void SetStatus(int i)
        {
            base.SetStatus(i);
            if (previousStatus != Status)
            {
                previousStatus = Status;
                Publish(previousStatus);
            }
        }
    }

### TrafficParticipants

Deze hebben geen mqtt connectie dus staan helemaal los van het SdmClient systeem, dit zijn in principe gewoon unity objecten die reageren op objecten (zoals een rood stoplight)

Deze worden gespawned door `Path.cs` deze staat hieronder uitgelegd.

### Car.cs

Een Car is een `TrafficParticipant.cs`, alle logica van het bewegen (omdat alles gewoon beweegt jwz) staat in `TrafficParticipant.cs`.

### Path.cs

Maak een leeg object, gooi deze in de scene en atta.tch het `Path.cs` script er aan, vul alles in in de inspector en het zal moeten werken (misschien wil iemand de spawning tweaken :)).

###Enums

Spreken voor zich.

###SdmManager

Moet een beetje de algemene stand van zaken van de scene bijhouden (bijv hoeveel trafficobjecten er op dat moment zijn.

###UnityThread.cs

Dit is een klasse die er voor zorgt dat je multithreaded dingen kan doen binnen Unity, gebruik:

Let op, een thread word gestart in SdmClient, dus alles met mqtt connectie gebruikt dit automatisch:

     private void Awake()
        {
            UnityThread.initUnityThread();
        }

Enige wat jij moet doen (voorbeeld stoplight):

    public void UpdateStopLight()
    {
        UnityThread.executeInUpdate(() =>
            SetRendererColor(GetColorByStatus(Status))
        );
    }

Bedankt en veel plezier.
