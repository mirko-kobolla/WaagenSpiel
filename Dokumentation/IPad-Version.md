# Waagenspiel auf iPad und iPhone

Diese Datei beschreibt, wie die iPad-Version des Waagenspiels aufgebaut wurde, was im Projekt geaendert wurde und wie das Spiel von einem Windows-Laptop auf einem iPad oder iPhone gestartet werden kann.

## 1. Ausgangssituation

Die urspruengliche Anwendung ist eine WPF-Desktop-Anwendung fuer Windows:

- Projektdatei: `WaagenSpiel.csproj`
- Zielplattform: `.NET Framework 4.7.2`
- Benutzeroberflaeche: XAML und WPF-Code-behind
- Startfenster: `MainWindow.xaml`
- Spielfenster: `SpielWindow.xaml`

WPF ist eine Windows-Technologie. Eine WPF-Anwendung kann deshalb nicht direkt auf iPadOS oder iOS ausgefuehrt werden.

Die vorhandene Spielidee und Teile der Spiellogik sind dagegen nicht an Windows gebunden. Diese Teile konnten fuer eine neue Webanwendung wiederverwendet werden.

## 2. Eigener Git-Branch

Fuer die Portierung wurde ein eigener Branch erstellt:

```text
IPad-Version
```

Git-Branch-Namen duerfen keine Leerzeichen enthalten. Der gewuenschte Name wurde deshalb als `IPad-Version` angelegt.

Der Implementierungsstand wurde gespeichert als:

```text
4484815 iPad-Version als Blazor-PWA implementieren
```

Die urspruengliche Windows-Version bleibt im Branch `main` erhalten.

## 3. Gewaehlte Technik: Blazor WebAssembly PWA

Die iPad-Version ist eine **Blazor-WebAssembly-PWA**.

### Blazor WebAssembly

Blazor WebAssembly ermoeglicht, C#-Code im Browser auszufuehren. Dadurch koennen Teile der bestehenden C#-Spielmodelle weiterverwendet werden.

### PWA

PWA bedeutet Progressive Web App. Eine PWA:

- wird im Browser aufgerufen,
- kann auf dem Home-Bildschirm installiert werden,
- kann ein eigenes App-Symbol und einen eigenen Namen haben,
- kann mit einem Service Worker auch offline funktionieren.

Eine PWA ist trotzdem keine native Swift-App aus dem App Store. Sie wird ueber Safari oder einen anderen Browser installiert.

## 4. Neue Projektstruktur

Das neue Projekt liegt im Unterordner `IPadVersion`:

```text
IPadVersion/
  IPadVersion.csproj       Blazor-WebAssembly-Projekt
  Program.cs               Startpunkt der Webanwendung
  App.razor                Routing der Razor-Komponenten
  Pages/Home.razor         Spieloberflaeche und Spielablauf
  Layout/MainLayout.razor  Einfaches Layout ohne Desktop-Seitenleiste
  wwwroot/
    css/app.css            Responsive Gestaltung fuer Touchgeraete
    index.html             Browser-Einstiegspunkt
    manifest.webmanifest   Name, Farben und Icons der PWA
    service-worker.js      Offline-Unterstuetzung im Entwicklungsbetrieb
    service-worker.published.js
```

Die Beispielseiten des Blazor-Templates bleiben im Projekt, werden aber fuer das Spiel nicht verwendet. Die Startseite ist `Pages/Home.razor`.

## 5. Wiederverwendung der Spiel-Logik

Die vorhandenen Klassen werden im neuen Projekt verknuepft, statt sie zu kopieren. Das geschieht in `IPadVersion/IPadVersion.csproj`:

```xml
<Compile Include="..\Models\Stein.cs" Link="Spielkern\Stein.cs" />
<Compile Include="..\Models\Gruppe.cs" Link="Spielkern\Gruppe.cs" />
<Compile Include="..\Models\Waage.cs" Link="Spielkern\Waage.cs" />
<Compile Include="..\Spiel\SpielManager.cs" Link="Spielkern\SpielManager.cs" />
```

Dadurch verwenden WPF-Version und iPad-Version dieselben Dateien:

- `Models/Stein.cs`: Farbe, Gewicht und Platzierungsstatus eines Steins
- `Models/Gruppe.cs`: zehn Steine einer Gruppe und deren Status
- `Models/Waage.cs`: linke und rechte Seite, Gewichtsberechnung und Gleichgewicht
- `Spiel/SpielManager.cs`: Gruppenverwaltung und Wechsel zur naechsten Gruppe

Das ist wichtig, weil die Regeln nicht zweimal unabhaengig gepflegt werden muessen.

## 6. Was die neue Spieloberflaeche kann

In `IPadVersion/Pages/Home.razor` wurde die Demo-Startseite durch das eigentliche Spiel ersetzt.

Die Bedienung funktioniert so:

1. Einen farbigen Stein antippen.
2. Eine Zielseite auswaehlen: Waage 1 oder 2, jeweils links oder rechts.
3. Weitere Steine platzieren.
4. `Kombination prüfen` antippen. Das Spiel berechnet die Gewichte intern, zeigt sie aber nicht an.
5. Wenn eine Waage gleich schwer ist und die Farbkombinationen nicht identisch sind, wird die Gruppe als geloest markiert.
6. Mit `Nächste Gruppe` oder `Vorherige Gruppe` kann die aktive Gruppe gewechselt werden. Nach einem Treffer wechselt das Spiel automatisch zur nächsten Gruppe.
7. Mit `Neue Partie` kann das Spiel jederzeit zurueckgesetzt werden.

Die beiden Waagen heißen in der iPad-Version **Hauptwaage** und **Nebenwaage**. Die exakten Grammwerte werden absichtlich nicht angezeigt, damit die Spieler die Kombination aus den sichtbaren Vergleichsergebnissen selbst ermitteln.

Die Oberflaeche wurde fuer Touch angepasst:

- grosse Schaltflaechen,
- responsive Darstellung fuer kleine Bildschirme,
- Hoch- und Querformat-taugliches Layout,
- sichtbare Statusmeldung,
- farbliche Darstellung der Steine,
- keine WPF-Fenster oder Desktop-Steuerelemente.

## 7. PWA-Konfiguration

In `IPadVersion/wwwroot/manifest.webmanifest` wurden Name, Kurzname, Farben und Symbole definiert:

```json
{
  "name": "Waagenspiel iPad-Version",
  "short_name": "Waagenspiel",
  "display": "standalone"
}
```

`display: standalone` sorgt dafuer, dass das Spiel nach der Installation moeglichst wie eine eigene App ohne normale Browser-Navigation startet.

Der Service Worker wird in `wwwroot/index.html` registriert. Nach einem veroeffentlichten Build kann er die benoetigten Dateien lokal zwischenspeichern.

## 8. Voraussetzungen auf dem Windows-Laptop

Auf dem Laptop benoetigt man:

1. .NET SDK 10 oder eine dazu passende SDK-Version.
2. Git ist fuer die Entwicklung empfohlen, aber zum reinen Starten der PWA nicht zwingend erforderlich.
3. Windows-Firewall muss eingehende Verbindungen fuer den lokalen Entwicklungsserver erlauben.
4. Laptop und iPad/iPhone muessen sich im selben WLAN befinden.

Die installierte Version dieses Projekts wurde mit .NET SDK 10 getestet.

## 9. Spiel im lokalen WLAN auf dem iPad testen

### 9.1 IP-Adresse des Laptops herausfinden

In PowerShell auf dem Windows-Laptop:

```powershell
ipconfig
```

Die relevante Adresse steht bei dem aktiven WLAN-Adapter hinter `IPv4-Adresse`, zum Beispiel:

```text
192.168.178.42
```

### 9.2 Entwicklungsserver starten

Im Projektordner des Repositories ausfuehren:

```powershell
dotnet run --project .\IPadVersion\IPadVersion.csproj --urls http://0.0.0.0:5187
```

Die Option `0.0.0.0` sorgt dafuer, dass der Server nicht nur fuer den Laptop selbst, sondern auch aus dem lokalen Netzwerk erreichbar ist.

### 9.3 Verbindung erlauben

Falls Windows beim ersten Start nach einer Firewall-Freigabe fragt, muss der Zugriff fuer private Netzwerke erlaubt werden. In einem oeffentlichen oder fremden WLAN sollte der Server nicht freigegeben werden.

Wenn das iPhone meldet, dass der Server nicht antwortet, sind meistens das Netzwerkprofil oder die Firewall die Ursache. Das WLAN muss als **Privat** eingestuft sein. PowerShell muss dafuer als Administrator gestartet werden. Danach koennen diese Befehle ausgefuehrt werden:

```powershell
Set-NetConnectionProfile -InterfaceAlias "WLAN" -NetworkCategory Private
New-NetFirewallRule -DisplayName "Waagenspiel iPad PWA 5187" -Direction Inbound -Action Allow -Protocol TCP -LocalPort 5187 -Profile Private
```

Anschließend den Entwicklungsserver neu starten und auf dem iPhone erneut diese Adresse öffnen:

```text
http://192.168.0.246:5187
```

Falls die Befehle wegen fehlender Administratorrechte abgelehnt werden, muessen sie in einem **als Administrator gestarteten** PowerShell-Fenster ausgefuehrt werden.

### 9.4 Auf dem iPad oder iPhone oeffnen

Auf dem iPad oder iPhone Safari oeffnen und folgende Adresse eingeben. Die IP-Adresse muss durch die Adresse des eigenen Laptops ersetzt werden:

```text
http://192.168.178.42:5187
```

Danach sollte das Waagenspiel im Browser erscheinen.

Der Laptop muss waehrend dieses Tests eingeschaltet bleiben und der Entwicklungsserver muss weiterlaufen.

## 10. Auf dem Home-Bildschirm installieren

Wenn die Anwendung im Safari-Browser geoeffnet ist:

1. Das Teilen-Symbol antippen.
2. `Zum Home-Bildschirm` auswaehlen.
3. Den Namen bestaetigen.
4. Das neue Symbol `Waagenspiel` oeffnen.

Wichtig: Im einfachen lokalen HTTP-Testbetrieb ist die PWA-Installation und insbesondere Offline-Nutzung auf iOS nicht in jeder Safari-Version garantiert. Fuer einen verlaesslichen installierbaren Offline-Betrieb sollte die Anwendung ueber HTTPS bereitgestellt werden.

## 11. Verlaesslicher Betrieb ueber HTTPS

Fuer den dauerhaften Betrieb sollte die PWA auf einem Webserver mit HTTPS liegen. Moegliche Varianten sind:

- ein HTTPS-Webhosting,
- GitHub Pages oder ein anderer statischer Hostingdienst,
- ein eigener Webserver im Netzwerk mit vertrauenswuerdigem Zertifikat.

Release-Dateien erzeugen:

```powershell
dotnet publish .\IPadVersion\IPadVersion.csproj -c Release -o .\IPadVersion\publish
```

Der Inhalt des Publish-Ergebnisses muss auf einem Webserver bereitgestellt werden. Die Adresse muss per `https://` erreichbar sein.

Beim ersten Aufruf mit Netzwerkverbindung laedt Safari die Anwendung und ihre Dateien. Danach kann der Service Worker die Dateien fuer einen spaeteren Offline-Aufruf verwenden.

Ein selbst signiertes Zertifikat auf dem Windows-Laptop kann fuer private Tests genutzt werden, muss aber auf dem iPad als vertrauenswuerdig eingerichtet werden. Fuer Einsteiger ist ein echtes HTTPS-Hosting meist einfacher und sicherer.

## 12. Unterschied zwischen Testbetrieb und echter App

### Lokaler Testbetrieb

```text
Laptop + Entwicklungsserver + gleiches WLAN + Safari
```

Vorteile:

- schnell eingerichtet,
- keine Veroeffentlichung notwendig,
- gut fuer Entwicklung und Tests.

Nachteile:

- Laptop muss laufen,
- URL basiert auf der lokalen IP-Adresse,
- HTTP ist fuer PWA-Offlinefunktionen auf iOS eingeschraenkt.

### Veroeffentlichte PWA

```text
HTTPS-Webadresse + Safari + Zum Home-Bildschirm
```

Vorteile:

- installierbares App-Symbol,
- bessere Offline-Unterstuetzung,
- Laptop muss nicht dauerhaft laufen,
- auf iPad und iPhone nutzbar.

Nachteile:

- Hosting wird benoetigt,
- bei oeffentlichem Hosting muessen Sicherheit und Zugriff bedacht werden.

## 13. Bekannte Grenzen des aktuellen Standes

- Die iPad-Version ist eine Web-App, keine native App-Store-Anwendung.
- Der aktuelle Spielstand wird noch nicht dauerhaft zwischen Geraeten synchronisiert.
- Ein Neustart kann den Spielstand verlieren, wenn keine lokale Speicherung implementiert ist.
- Mehrere Personen oder mehrere Geraete spielen noch nicht gemeinsam online.
- Die bestehende WPF-Version bleibt eine separate Windows-Anwendung.
- Die vorhandene WPF-Anwendung benoetigt weiterhin Visual Studio beziehungsweise eine funktionierende WPF-Buildumgebung.

## 14. Nützliche Befehle

Projekt wiederherstellen:

```powershell
dotnet restore .\IPadVersion\IPadVersion.csproj
```

Debug-Build:

```powershell
dotnet build .\IPadVersion\IPadVersion.csproj
```

Lokalen Server starten:

```powershell
dotnet run --project .\IPadVersion\IPadVersion.csproj --urls http://0.0.0.0:5187
```

Release-Paket erstellen:

```powershell
dotnet publish .\IPadVersion\IPadVersion.csproj -c Release -o .\IPadVersion\publish
```

Branch anzeigen:

```powershell
git branch --show-current
```

## 15. Kurzfassung fuer den ersten Test

1. Laptop und iPad in dasselbe private WLAN bringen.
2. In PowerShell `ipconfig` ausfuehren und die IPv4-Adresse notieren.
3. Im Repository `dotnet run --project .\IPadVersion\IPadVersion.csproj --urls http://0.0.0.0:5187` starten.
4. Auf dem iPad `http://LAPTOP-IP:5187` in Safari oeffnen.
5. Steine antippen und auf den Waagen platzieren.
6. Spaeter fuer echte Offline-Installation ein HTTPS-Hosting verwenden.
