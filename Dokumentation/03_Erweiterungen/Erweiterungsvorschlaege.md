# Erweiterungsvorschlaege fuer die 8. Klasse

## 1. Frei waehlbare Anzahl der Gruppen

Die Spielleitung oder der Benutzer waehlt die Gruppenzahl beim Start. Die Obergrenze sollte von der Anzahl verfuegbarer Steine und der Bildschirmdarstellung abhaengen.

**Vorschlag:** Erlaube 2 bis 10 Gruppen. Bei jeder Gruppe werden ein Name, die Steine und der Status gespeichert. Die Reihenfolge wird danach wie bisher festgelegt.

## 2. Namen fuer Gruppen

Beim Spielstart kann jede Gruppe einen eigenen Namen eingeben, zum Beispiel "Die Denker". Der Name erscheint in der Reihenfolge, bei Statusmeldungen und in der Abschlussuebersicht. Eine leere Eingabe sollte automatisch einen Namen wie "Gruppe 1" erhalten.

## 3. Sind frei waehlbare Steinmengen loesbar?

### Empfehlung

Die Anzahl der Steine sollte waehlbar sein, aber nicht voellig unbeschraenkt. Eine Einstellung von 2 bis 5 Steinen pro Gruppe ist fuer eine 8. Klasse gut handhabbar. Das Programm muss unloesbare Einstellungen verhindern oder deutlich warnen.

### Begruendung

Jeder zusaetzliche unbekannte Stein ist eine weitere Variable. Die Waagen liefern zwar Gleichungen, aber nicht jede Anordnung liefert eine neue, unabhaengige Information. Zu viele Steine koennen daher zu mehreren moeglichen Loesungen oder zu keiner erreichbaren Loesung fuehren.

### Technischer Vorschlag

- Mindestanzahl pro Gruppe: 2 Steine, passend zur Zugregel.
- Empfohlene Standardeinstellung: 2 Steine pro Gruppe.
- Maximalwert: 5 Steine pro Gruppe.
- Vor Spielbeginn erzeugt das Programm ein Gewichtsszenario und prueft, ob die Werte eindeutig bestimmbar sind.
- Bei keiner eindeutigen Loesung wird ein anderes Szenario erzeugt oder eine Warnung angezeigt.
- Die Spielleitung kann zwischen "ein Gewicht je Farbe" und "eigenes Gewicht je Stein" waehlen. Diese Entscheidung muss sichtbar sein, weil sie die Schwierigkeit stark veraendert.

## 4. Tutorial-Modus mit Loesungshinweisen

Ein Tutorial fuehrt die Spieler in kurzen Schritten durch eine Beispielrunde. Es zeigt nicht sofort die Loesung, sondern gibt abgestufte Hinweise:

1. Welche Information liefert die aktuelle Waage?
2. Welche Steine koennen verglichen werden?
3. Welche Gewichte sind dadurch ausgeschlossen?
4. Welche konkrete Rechnung fuehrt zum naechsten Gewicht?
5. Auf Wunsch die vollstaendige Loesung anzeigen.

Der Tutorial-Modus sollte ein eigenes Spiel starten, damit die echte Runde nicht veraendert wird. Ein Fortschrittsbalken und ein Zurueck-Button erleichtern das Lernen.

## 5. Uebersicht am Ende jeder Runde

Nach jeder Runde wird eine kompakte Tabelle angezeigt:

| Gruppe | Noch nicht gespielte Steine | Status |
|---|---|---|
| Name der Gruppe | Farben oder Stein-IDs | aktiv / ausgeschieden / fertig |

Die Ansicht sollte vor dem naechsten Zug bestaetigt oder automatisch fuer einige Sekunden angezeigt werden. Nicht mehr verfuegbare Steine werden klar von bereits platzierten Steinen unterschieden.

## Weitere passende Erweiterungen

- **Schwierigkeitsstufen:** Einsteiger mit Gewichtsbereich 1 bis 10 g, Fortgeschrittene mit 1 bis 20 g und Experten mit mehr Steinen.
- **Hinweisbudget:** Jede Gruppe bekommt zum Beispiel drei Hinweise. Dadurch bleibt Hilfe moeglich, ohne das logische Loesen abzunehmen.
- **Punkte statt nur Sieg oder Niederlage:** Punkte fuer richtige Gewichte, wenige Hinweise und eine ausbalancierte Hauptwaage.
- **Lehrkraft-Modus:** Die Lehrkraft legt Gewichte, Gruppenzahl, Steinanzahl und Zeitlimit fest.
- **Zeitlimit pro Zug:** Eine sichtbare Uhr trainiert schnelle Entscheidungen und verhindert lange Wartezeiten.
- **Automatische Regelpruefung:** Das Programm prueft vor dem Start, ob alle Gruppen mindestens zwei Steine erhalten und ob die Runde loesbar ist.
- **Auswertung am Ende:** Die Anwendung zeigt richtige und falsche Annahmen sowie die benoetigten Waagenvergleiche an.
- **Barrierearme Darstellung:** Farben werden zusaetzlich durch Symbole oder Namen gekennzeichnet, damit die Aufgabe nicht nur von Farberkennung abhaengt.
- **Speichern und Fortsetzen:** Eine Runde kann unterbrochen und spaeter fortgesetzt werden.

## Prioritaet fuer die Umsetzung

1. Gruppennamen und frei waehlbare Gruppenzahl
2. Rundenabschluss-Uebersicht
3. Automatische Loesbarkeitspruefung bei waehlbarer Steinanzahl
4. Tutorial mit abgestuften Hinweisen
5. Punkte, Zeitlimit und weitere Spielmodi

## 6. Gruppen in einem dauerhaften Rundlauf

Nach dem Zug der letzten Gruppe soll automatisch wieder die erste Gruppe an die Reihe kommen. Die Waagen bleiben dabei bestehen und werden nicht geleert. Dadurch entsteht ein gemeinsamer Spielstand, an dem alle Gruppen nacheinander weiterarbeiten.

Mögliche spätere Erweiterungen:

- Anzeige der bisherigen Zugreihenfolge und der Anzahl absolvierter Runden
- Ein sichtbarer Hinweis, welche Gruppe als Nächstes an der Reihe ist
- Optional eine Begrenzung der Rundenzahl oder ein Zeitlimit für die gesamte Partie
- Eine Übersicht, wie viele Steine jede Gruppe bereits auf den Waagen platziert hat

## 7. Gewichte nach einer ausgeglichenen Hauptwaage raten

Wenn die Hauptwaage ausgeglichen ist, kann die Spielleitung oder die aktive Gruppe über einen eigenen Button die Gewichte der Farben raten. Für jede Farbe wird ein vermutetes Gewicht eingetragen und anschließend gemeinsam geprüft.

Die Anwendung sollte dabei:

- den Button nur bei einer belegten und ausgeglichenen Hauptwaage anzeigen,
- die exakten Gewichte weiterhin verborgen halten,
- eine verständliche Rückmeldung bei richtigen oder falschen Angaben geben,
- nach Möglichkeit anzeigen, welche Angaben bereits stimmen, ohne die übrigen Lösungen direkt zu verraten,
- die Gewichtsabfrage unabhängig vom Gruppenwechsel offen oder abgeschlossen halten können.

Für den Unterricht wäre zusätzlich sinnvoll, zwischen verschiedenen Auswertungen wählen zu können:

- Alle Gewichte müssen vollständig richtig sein.
- Ein Teil der richtigen Gewichte gibt Punkte.
- Jede Gruppe erhält nur eine begrenzte Anzahl von Rateversuchen.

## 8. Auswertung nach einer richtigen Gewichtsabfrage

Nach einer vollständig richtigen Gewichtsabfrage könnte eine Abschlussübersicht erscheinen. Sie zeigt die Farben, die erratenen Gewichte, die benötigten Versuche und die beteiligten Gruppen. So wird aus dem gemeinsamen Waagenstand eine nachvollziehbare mathematische Auswertung.
