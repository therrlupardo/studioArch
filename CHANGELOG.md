# Changelog

## Architect.cs
* usunięto pola:
  - `data urodzenia`
  - `numer telefonu`

* dodano pola:
  - `idPrzełożonego`
  - `pesel`
  - `data wstawienia`
  - `data wygaśnięcia`

* zmieniono wartości uprawnienia_do_nadzoru na `UPRAWNIONY`, `NIEUPRAWNIONY`
* poprawiono `ToCsvString()` zgodnie z powyższym

## Client.cs
* usunięto pola:
  - `telefon`
  - `mail`

* dodano pole `typ_klienta`

## Project.ts
* usunięto pole `status`

* dodano pola
  - `opóźnienie rozpoczęcia`
  - `opóźnienie zakończenia`
  - `łączna cena`
  - `czy nadzorowany`
  - `długość (enum)`