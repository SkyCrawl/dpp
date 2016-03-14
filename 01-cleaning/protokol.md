## Obecné změny

* Formátováno. Dodatečné poznámky:
    * 80 sloupců.
    * Explicitně doplněny závorky pro bloky sestávající z jediného příkazu.
    * Konvence velkých a malých písmen podle 
[MSDN](https://msdn.microsoft.com/en-us/library/ms229043.aspx).
Privátní metody začínají malým písmenem.
* Namespace přejmenován na `HuffmanCoding`.
* Ke všem fieldům/metodám explicitně doplněn scope.
* Obsah metod je nyní mnohdy explicitně komentován.
* `SortedDictionary<int, List<Node>>` představoval typ přečteného vstupu a byl
extrahován do speciálního typu (`FreqSortedHuffForests`), i protože metoda
čtení a přípravy tohoto vstupu byla dlouhá a zasloužila si lepší členění.
* Ze třídy `FreqSortedHuffForests` byly dále pro lepší srozumitelnost,
přehlednost a znovupoužitelnost extrahovány dva nové typy: `CharToFrequency` a
`HuffForests`.
* Třída `Nacitacka` byla zlikvidována a její obsah přesunut do `Program` a
`FreqSortedHuffForests`, kde dává větší smysl.
* Doplněno několik smysluplných `FIXME` a `TODO` komentářů.
* Některé public fieldy některých tříd jsou nyní jen pro čtení
(privátní setter), aby se nestala zbytečná nehoda.
* Program již nezabíjí konzoli (`Environment.Exit(0)`) a místo toho vrací
(`return;`).

## Třída 'Program'

* Kód měřící dobu běhu programu byl odkomentován a zaobalen metodou (důvod viz
níže).
* Kód programu byl zaobalen novou metodou (přímá integrace s novou metodou
měření doby běhu).
* Statické fieldy `vrcholy` a `Huffman` byly přemístěny do metody `Main`,
protože jsou pro ni lokální, vytváří se v ní jejich hodnota a není použita nikde
jinde.
* Definováno několik pomocných extenzí/pozlátek.
* Template pro vypisování doby běhu programu byl zkrácen.
* Vypisování doby běhu se již nepřiřazuje žádné proměnné (byla pouze vypisována
a nic se s ní reálně nedělalo).
* Vytvořena konstanta potřebného počtu argumentů pro program.
* Přejmenovány původní proměnné.
* Základní logika načítání souborů byla vylepšena, oproti původní verzi
programu se odchytávají výjimky v průběhu celého čtení vstupního souboru,
nejenom při jeho otevření.
* Volání metody `strom.VypisStrom()` bylo odstraněno, protože daná metoda
obsahuje jediný zakomentovaný řádek, volající neexistující metodu.

## Třída 'FreqSortedHuffForests'

* Upravena vzhledem k zavedení `CharToFrequency`.
* Vyřešeny potenciální problémy s `null` seznamy.

## Třída 'CharToFrequency'

* Magická čísla byla extrahována do statických proměnných.
* `Node[]` bylo změněno na typ třídy `Dictionary<byte, int>` a následně došlo
k eliminaci zbytečných proměnných. Trochu méně efektivní, nicméně příjemnější.
* Čtení vstupního souboru již probíhá jen v jednom cyklu.
* Proměnné byly přejmenovány.
* Odstraněn zakomentovaný kód, který již nedával smysl.

## Třída 'strom'

* Přejmenována na `HuffTree`.
* Přejmenován field `koren`, metody a argumenty metod.
* Metoda `postavStrom` sloužila pouze k inicializaci a byl to jediný příkaz
konstruktoru, takže byla spojena s konstruktorem. Následně došlo také k
rozdělení konstruktoru na více metod a výsledek byl výrazně zjednodušen.
* Proměnná `pocetStromu` byla společně se související logikou odstraněna,
protože nebyla použita a pouze obsahovala hodnotu 0 nebo 1.
* Metoda `VypisStrom()` byla zakomentována, protože obsahuje jediný
zakomentovaný řádek, volající neexistující metodu.
* Metoda `VypisStrom2(vrchol vrch, string pre)` byla výrazně zjednodušena.
* Zavedeny konstanty `MIN_ASCII_PRINTABLE` a `MAX_ASCII_PRINTABLE`.
* Odstraněny případné budoucí problémy s prázdnými soubory a `null` uzly
(včetně kořene).
* Udržování proměnné `ZbyvaZpracovat` jsme obešli speciální metodou ve třídě
`FreqSortedHuffForests`, efektivní pro toto konkrétní použití.

## Třída 'vrchol'

* Přejmenována na `HuffNode`.
* Přejmenovány a přeskupeny fieldy a argumenty konstruktoru.
* Přejmenovány metody.
* Nepoužitý kód byl seskupen a zakomentován.
* Metoda `IsLeaf` byla zkrácena (ternární operátor) a transformována na
Property.
* Argument `obj` metody `CompareTo` byl přejmenován na `other`.
* Metoda `CompareTo` byla zkrácena (ternární operátor).
* Metoda `shouldBeLeftOf` byla výrazně zjednodušena.

## Neprovedené změny

* Program by mohl brát ještě jeden argument, který bude určovat aplikaci stopek,
ale měl by být pro přehlednost a lepší použití první v pořadí, a tomu zabraňuje
zadání.
* Parametrizace třídy `HuffNode`, která by pouze obsahovala fieldy `data`, 
`leftChild` a `rightChild`. V rámci např. rozšíření programu na knihovnu by se
změna mohla hodit, nicméně také zvyšuje zavlečenou složitost a zatím nemá smysl.
* Aplikace návrhového vzoru "Factory" na třídu `HuffNode` (její stáří/rank
přímo naznačuje její užitečnost). Podobný argument jako v předchozím případě.
* Ve třídě `HuffForests` by skoro určitě byla místo `List` vhodnější třída
`SortedSet`. Pro některé vstupy by se vší pravděpodobností ale měla za
následek jinou výslednou podobu stromu, a to nám zadání úkolu zakazuje.
* Vylepšení rozhraní - `HuffTree` bychom mohli (či dokonce měli) použít ve třídě
`HuffForest`, ale to by mělo dlouhosáhlé důsledky a výsledný kód by byl ještě
více nerozeznatelný od originálu.
* Konstruktory tříd `HuffTree`, `FreqSortedHuffForests` a `CharToFrequency`
obsahují složitější logiku, pomocí které se samy inicializují. Pravděpodobně to
není zcela podle DPP, nicméně v tomto případě je to únosné a kód může být o
něco deklarativnější, bez dalších zbytečných metod.
* Při vypisování běhu programu se vypisují minuty, sekundy, a k tomu celkový
počet milisekund. Nevíme proč a tudíž neopraveno.
* Všechna `Console.Write` volání zapisující na konci `\n` byl mohla být
přepsána na `Console.WriteLine`, ale to by změnilo chování programu ve Windows,
kde se standardně ukončují řádky pomocí `\r\n`.
