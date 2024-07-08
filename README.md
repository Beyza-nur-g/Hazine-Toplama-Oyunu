# Hazine-Toplama-Oyunu
Otomat olarak görüş alanı içerisindeki en yakın hazineleri toplama oyunudur. oyun mantığı olarak hazine toplama sırasında karakterin karşısına engeller çıkmaktadır ve karakter bu engellerin etrafından dolaşarak ya da zıplayarak engelleri aşması beklenir.

Engel türleri hareketli ve hareketsiz olarak
ikiye ayrılmaktadır . hareketli engeller olarak 3 birim sag - ˘
sol hareket eden arı ve 5 birim yukarı- a¸sagı hareket eden ku¸s ˘
bulunuyor. hareketsiz olarak ; agaç,duvar,kaya ve dag olmak ˘
üzere dört çe¸sit hareketsiz engel bulunuyor. Hareketsiz engeller
yaz ve kı¸s teması olarak ikiye ayrılmaktadır. Ayrımları haritanın içerisindeki bulundukları noktaya göre ayarlanmı¸stır. Bu
¸sekilde haritanın yarısında yaz temalı engeller bulunuyorken
diger yarısında kı¸s temalı engeller bulumaktadır. Karekterin ˘
görü¸s alanı içerisine giren engeller bir listeye yazdırılıyor
. Aynı ¸sekilde toplanan hazineler de yazdırılıyor. Yazdırma
i¸slemi altın - bakır - gümü¸s - zümrüt ¸seklinde bir sıralama
benimsemi¸stir. .

Oyun c sharp dilinde microsoft Vsiual Stduio editörü ile yazılmı¸stır. Oyun , editörün saglamı¸s oldu ˘ gu araçlardan windows ˘
form üzerine yazılmı¸stır. Bir timer yardımıyla oyun hareketleri
saglanmaktadır. hareketli engeller ve karakterin birimkare ha- ˘
reketi için gerekli fonksiyonlar kullanılmı¸stır. Bu kısımlar için
ayrı ayrı picture box nesneleri olu¸sturulup kontroller bu ¸sekilde
saglanmı¸stır. Oyun nesne yönelik programlamaya uygun bir ˘
¸sekilde tasarlanmı¸s olup bütün engellerin , karakterlerin ve
hazinlerin kendilerine özgü classları bulunmaktadır.Classlardan
olu¸sturulan nesneler ile oyun sırasında karaktere yön verme
engelleri ve hazineleri olu¸sturma i¸slemleri gerçekle¸stirilmi¸s¸stir.


III. YÖNTEM
Öncelikle oyun sırasında sürekli olarak kullanılacak classlar olu¸stulmu¸stur. Bu classlar : Lokasyon , Oyun karakteri
,Hazine ,Engel Lokasyon classının içinde harita üzerindeki
konumları kullanabilmek için x ve y degerleri tutulmaktadır. ˘
Bu ¸sekilde kod içerisinde bir nesnenin konumunu ögrenilebilir. ˘
Oyun karakteri classınd id, ad , mevcut konum (lokasyondan
alınır.) ve toplanan hazine listesi tutulmaktadır.Bu class da
hareket etme fonksiyonu ve hazine toplama fonksiyonu bulunmkatadır. hazine classı içerisinde tur , pozisyon (lokasyon)
ve resim bulundurur. hazine classı diger hazine türlerine miras ˘
verir. tur degi¸skenimiz bu yüzden vardır. Altın , gümü¸s ,bakır ˘
ve zümrüt için ayrı classlar olu¸sturulur ve hazine classından
miras alırlar. ˙Içlerinde isimlerini ,resimlerini ve konumlarını
barındırırlar . Engel classı büyük bir classtır . Tür , boyut
, pozisyon ve resim degi¸skenlerini tutuyor . Engel classı ˘
hareketli engel ve harekestiz engel olarak ikiye ayrılıyor.
hareketli engelde hareketli engellere miraz bırakıyor. ku¸s ve
arı classında hareket etme için a¸sagı ve yukarı fonksiyonları ˘
bulunuyor. hareketsiz engeller classında mevsime göre resim
ayarlama fonksiyonu bulunuyor.duvar hareketsiz engeli için
ayrı bir resim olu¸sturma fonksiyonu kullanılıyor. Bunun sebebi
duvar resminin diger engellerde farklı olmasıdır. ˘

A. Fonksiyonlar
Hareket et ku¸s ve arı fonksiyonları hareketli engellerin
belirli bi döngüde hareket etmelerini saglar. kontrol et hareket ˘
et fonksiyonu en yakın hazine bul fonksiyonunu çagır ve ˘
en yakın hazine bulunur. hazine bulundugunda yönlendirme ˘
fonksiyonu çagırılır. Hazine konumu ile karakter konumu aynı ˘
oldugunda hazine topla fonksiyonu ile toplanan hazine listesine ˘
hazine atılır . Toplanan hazineler için bilgilendirme listboxına
hazine eklenir. Hazine bulunmazsa random harket fonksiyonu
çagırılır ve karakter hareket ettirilir. Timer her tetiklendi ˘ ginde ˘
karakterin kareket ettigi birimkareleri boyanması için boya ˘
yeri kırmızı fonksiyonu çagırılır. Aynı zamanda oyun tüm ˘
hazineler toplandıgında bitmesi için tüm hazineler toplandı ˘
fonksiyonun kontrolü yapılır. Ba¸slat butonu timer ba¸slatır ve
harita temizleme fonksiyonu ile harita temizlenir.


IV. DENEYSEL SONUÇLAR
Karakterimiz görü¸s alanı içerisindeki en yakın hazineyi
buldugunda konuma ilerlemesi gerekiyor. ˘
˙Ilerleme sırasında
önüne gelen engelleri a¸sması gerekiyor. A¸smak için kontroller
yapılır ve sonunda bir tarafa dogru yönlendirme yapar bu ˘
yönlendirme sonucunda engel boyutunu a¸sması beklenir. Bu
¸sekilde hazineyi toplamaya çalı¸sılır. Eger görü¸s alanı içerisinde ˘
bir hazine yoksa random hareket etme fonksiyonu ile gidebilecegi yönler bir list de tutulur ve listten random bir seçme ˘
ile hareket eder. Her hareket sonucunda kırmızı ile boyama
yapılarak karakteri takip edilebilir.


SONUC
Karakterimiz görü¸s alanı içerisindeki en yakın hazineyi
buldugunda o hazinenin konumuna en yakın yolu bulur ve ˘
engelleri a¸sarak hazineye ula¸sır . Bu sırada görü¸s alanındaki
engelleri listBox a yazdırır. Toplanan hazineleri de listboxa
yazdırır. Yazdırma i¸slemini isim ve konum ¸seklinde yapar.

