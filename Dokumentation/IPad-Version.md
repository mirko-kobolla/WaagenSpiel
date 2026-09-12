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
2. Eine Zielseite auswaehlen: Hauptwaage oder Nebenwaage, jeweils links oder rechts.
3. Weitere Steine platzieren.
4. `Loesung pruefen` antippen.
5. Wenn die Hauptwaage gleich schwer ist und die Farbkombinationen nicht identisch sind, wird die Gruppe als geloest markiert.
6. Mit `Neue Partie` kann das Spiel jederzeit zurueckgesetzt werden.

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

## 16. Aktuelle Bedienung und Spielablauf

Die Waagen heissen in der Anwendung jetzt:

- **Hauptwaage** statt Waage 1
- **Nebenwaage** statt Waage 2

Die konkreten Gewichte werden nicht mehr angezeigt. Unter den Seiten steht nur noch `Gewicht verborgen`; im Kopf der Waage wird stattdessen `Linke Seite schwerer`, `Rechte Seite schwerer`, `Im Gleichgewicht` oder bei einer leeren Waage `Noch keine Steine platziert` angezeigt. Das Spiel soll dadurch ueber die Farbkombinationen und den relativen Vergleich geloest werden, nicht durch direktes Ablesen der Zahlen.

### Einen Stein platzieren

1. Einen verfuegbaren farbigen Stein antippen.
2. Eine Seite der Hauptwaage oder Nebenwaage auswaehlen.
3. Den Vorgang fuer weitere Steine wiederholen.

Ein platzierter Stein ist danach deaktiviert und kann in dieser Runde nicht noch einmal verwendet werden.

Auf der Hauptwaage darf keine identische Farbkombination auf beiden Seiten entstehen. Wenn ein Platzieren zu derselben Kombination links und rechts fuehren wuerde, wird der Stein nicht platziert und das Statusfeld erklaert den Grund. Diese Einschraenkung gilt nicht fuer die Nebenwaage.

### Was bedeutet `Hauptwaage pruefen`?

`Hauptwaage pruefen` beendet den Zug. Der Button ist erst aktiv, wenn mindestens zwei Steine auf den Waagen liegen. Er prueft ausschliesslich die Hauptwaage, ob sie:

- links und rechts gleich schwer ist und
- auf beiden Seiten nicht dieselbe Farbkombination liegt.

Das Gewicht wird weiterhin intern berechnet, aber nicht auf dem Bildschirm ausgegeben. Bei einer falschen Kombination erscheint eine entsprechende Statusmeldung und die Gruppe bleibt aktiv.

Der Bereich oben ist das **Spielstatusfeld**. Er zeigt Rueckmeldungen zur aktuellen Aktion, zum Beispiel welcher Stein ausgewaehlt wurde, ob eine Platzierung abgelehnt wurde, ob die Hauptwaage noch nicht geloest ist oder welche Gruppe als naechste an der Reihe ist.

### Wie wechseln die Gruppen?

Der Gruppenwechsel erfolgt automatisch:

1. Die Steine der aktuellen Gruppe auf den Waagen verteilen.
2. `Loesung pruefen` antippen.
3. Bei einer gueltigen Loesung wird die aktuelle Gruppe als geloest markiert.
4. Die Waagen werden geleert.
5. Die naechste noch aktive Gruppe wird automatisch geladen.

Die aktuelle Gruppennummer steht oben rechts. `Neue Partie` setzt alle drei Gruppen zurueck und beginnt wieder mit Gruppe 1.

## 17. Wenn das iPhone den Server nicht erreicht

Wenn die Seite auf dem Laptop funktioniert, Safari auf dem iPhone aber meldet, dass der Server nicht antwortet, liegt das meist an Netzwerk oder Firewall, nicht am Spiel selbst.

### 17.1 Richtige IP-Adresse verwenden

Die IP-Adresse muss aus dem aktiven WLAN-Adapter des Laptops stammen. In PowerShell:

```powershell
Get-NetIPAddress -AddressFamily IPv4 |
  Where-Object { $_.IPAddress -notlike '127.*' -and $_.IPAddress -notlike '169.254.*' }
```

Eine gueltige IPv4-Adresse hat vier Zahlen zwischen 0 und 255, zum Beispiel:

```text
192.168.0.246
```

Adressen wie `10.0.26200.9168` sind ungueltig, weil einzelne Bestandteile groesser als 255 sind.

### 17.2 Gleiches WLAN pruefen

Der Laptop und das iPhone muessen im selben privaten WLAN angemeldet sein. Das iPhone darf nicht ueber Mobilfunk, ein Gast-WLAN oder ein VPN verbunden sein. Bei einem Gast-WLAN kann die FritzBox die Kommunikation zwischen den Geraeten absichtlich blockieren.

### 17.3 Server richtig starten

Der Server muss auf allen Netzwerkadressen lauschen:

```powershell
dotnet run --project .\IPadVersion\IPadVersion.csproj --urls http://0.0.0.0:5187
```

Auf dem iPhone wird anschliessend diese Form verwendet:

```text
http://LAPTOP-IP:5187
```

Beispiel:

```text
http://192.168.0.246:5187
```

Wichtig: Fuer den aktuellen lokalen Entwicklungsserver muss die Adresse mit `http://` beginnen, nicht mit `https://`. Der Server verwendet derzeit keine TLS-Verschluesselung. Wenn Android meldet, dass keine verschluesselte Verbindung unterstuetzt wird, wurde wahrscheinlich `https://192.168.0.246:5187` eingegeben. Richtig ist:

```text
http://192.168.0.246:5187
```

Eine verschluesselte Verbindung fuer den dauerhaften Betrieb benoetigt ein HTTPS-Zertifikat und wird im Abschnitt 11 beschrieben. Ein lokales .NET-Entwicklungszertifikat wird von iPhone und Android nicht automatisch als vertrauenswuerdig akzeptiert.

### 17.4 Windows-Firewall freigeben

Wenn der Server lokal funktioniert, aber das iPhone keine Verbindung bekommt, muss TCP-Port 5187 fuer eingehende Verbindungen freigegeben werden. PowerShell muss dafuer als Administrator gestartet werden:

```powershell
New-NetFirewallRule `
  -DisplayName "Waagenspiel PWA 5187" `
  -Direction Inbound `
  -Protocol TCP `
  -LocalPort 5187 `
  -Action Allow `
  -Profile Private
```

Das WLAN-Profil sollte fuer diesen Test `Private` sein. Der Status kann geprueft werden:

```powershell
Get-NetConnectionProfile
```

Ein oeffentliches Netzwerkprofil sollte nicht einfach freigegeben werden. Wenn es sich um das eigene vertrauenswuerdige WLAN handelt, kann das Profil in den Windows-Einstellungen auf `Privat` gestellt werden. Danach den Server neu starten und die iPhone-Adresse erneut aufrufen.

Das aktuelle WLAN-Profil dieses Laptops ist `Public`. Deshalb muss die Firewallregel in einer **als Administrator gestarteten PowerShell** mit `-Profile Any` oder nach dem Umstellen des eigenen WLANs auf `Private` angelegt werden. Ohne diese Freigabe kann der Laptop selbst die Seite oeffnen, waehrend iPhone und Android keine Verbindung erhalten.

#### Uebergabe an den Administrator

Die Firewallfreischaltung wurde auf diesem Laptop noch **nicht** ausgefuehrt, weil fuer das aktuelle Benutzerkonto keine Administratorrechte vorhanden sind. Der folgende Auftrag kann an den zustaendigen Administrator weitergegeben werden:

1. PowerShell mit `Als Administrator ausfuehren` starten.
2. Wenn das eigene WLAN als vertrauenswuerdig eingestuft werden darf, das Netzwerkprofil auf `Private` setzen.
3. Diese Regel nur fuer den Entwicklungsport des Waagenspiels anlegen:

```powershell
New-NetFirewallRule `
  -DisplayName "Waagenspiel PWA 5187" `
  -Direction Inbound `
  -Protocol TCP `
  -LocalPort 5187 `
  -Action Allow `
  -Profile Private
```

Falls das Netzwerkprofil auf `Public` bleiben muss, darf der Administrator die Freigabe bewusst auf dieses Profil erweitern. Das sollte nur im eigenen, kontrollierten WLAN geschehen:

```powershell
Set-NetFirewallRule `
  -DisplayName "Waagenspiel PWA 5187" `
  -Profile Any
```

Die Regel kann anschliessend so kontrolliert werden:

```powershell
Get-NetFirewallRule -DisplayName "Waagenspiel PWA 5187" |
  Get-NetFirewallPortFilter
```

Nach der Freigabe den PWA-Server neu starten und auf iPhone oder Android testen:

```text
http://192.168.0.246:5187
```

Nach dem Test kann der Administrator die Regel wieder entfernen:

```powershell
Remove-NetFirewallRule -DisplayName "Waagenspiel PWA 5187"
```

### 17.5 Port testen

Auf dem Laptop kann geprueft werden, ob der Server laeuft:

```powershell
Test-NetConnection 127.0.0.1 -Port 5187
```

`TcpTestSucceeded : True` bestaetigt nur den lokalen Zugriff. Fuer den iPhone-Test muessen zusaetzlich gleiches WLAN und Firewall stimmen.

## 18. Aenderungen dieser Version

In dieser Version wurden folgende Punkte umgesetzt:

- `Waage 1` wurde in `Hauptwaage` umbenannt.
- `Waage 2` wurde in `Nebenwaage` umbenannt.
- Die Anzeige der exakten Gewichte wurde aus der Oberflaeche entfernt.
- `Auswertung anzeigen` wurde in `Loesung pruefen` umbenannt.
- Die Bedeutung der Loesungspruefung wird direkt in der Oberflaeche erklaert.
- Nach einer gueltigen Loesung wechselt das Spiel automatisch zur naechsten Gruppe.
- Der Hinweistext erklaert, dass `Neue Partie` alle Gruppen zuruecksetzt.
- Die iPhone-Verbindungsprobleme wurden als Netzwerk-/Firewall-Schritte dokumentiert.
