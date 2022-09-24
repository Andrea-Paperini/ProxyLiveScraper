Applicazione console che permette di fare WebScraping con C# Net 5

Riempire il file Fonti.txt con gli url (http e https) di liste di proxy che desideri raschiare.

Il programma raschierà tutte le fonti che gli sono state fornite ed estrae tramite regex i proxy.

Successivamente controlla se ogni proxy è vivo oppure morto.

I proxy vivi vengono salvati in un file che si resetta prima di partire chiamato Http Live "data giornaliera".txt

In ogni operazione ti terrà presente dei seguenti parametri (Fonti totali, Fonte corrente, proxy vivi, proxy morti, proxy corrente).

Extra ➡️ A fine esecuzione il programma invia il file finale tramite le Api di Telegram con un Bot (necessario ApiToken) in una chat a vostra scelta (necessario ChatId)


Nota: viene definito vivo un proxy che oltre a funzionare ha un tempo di latenza inferiore a 3 secondi.
