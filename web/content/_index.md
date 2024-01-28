---
title: "Main Page"
description: 'Krieg Eterna'
layout: single
---

<section class="no-gradient">
    <div class="main-section" style="padding-top: 0">
        <div class="top-flex">
            <picture>
                <img src="/images/battle-1600x1080-100.jpg" class="top-img">
            </picture>
            <div class="top-slanted">
                <div class="top-info-box">
                    <picture>
                        <img src="/images/Title.png">
                    </picture>
                    <div class="title-wrapper">
                        <h4 class="top-title">A Battle For The Ages</h4>
                    </div>
                    <div class="top-paragraph">
                        <p>Kings and Armies assemble in the fields of Europe. Whether it be for God, glory or gold, the Continent marches to war!</p>
                    </div>
                </div>
            </div>
        </div>
    </div>
</section>

<section id="new-cards-showcase" class="gradient even-gradient">
    <div class="main-section">
        <div class="sub-section">
            <div class="title-wrapper">
                <h2>Meet Your Army</h2>
            </div>
            <p class="css-tg8OC">
                Your units do the fighting, and your King and power cards make the
                round interesting! You’ll have to learn how to use them together in order to win.
                With over <a href="/compendium">100 unique cards</a>, the possibilities are endless!
            </p>
        </div>
        <div class="css-XorOV" style="--container-flex-direction:column-reverse;">
            <div class="css-JEZym" style="--intersection-offset:0;">
            </div>
                {{< card-scroll-gallery >}}
                    {{< cardflip front="Calvary" back="Flag" disallowScale="mobile" >}}
                    {{< cardflip front="TraitorKing" back="FlagKing" disallowScale="mobile" >}}
                    {{< cardflip front="Armada" back="Flag" disallowScale="mobile" >}}
                    {{< cardflip front="Saboteur" back="FlagPower" disallowScale="mobile" >}}
                    {{< cardflip front="Ruin" back="FlagPower" disallowScale="mobile" >}}
                    {{< cardflip front="Fortress" back="Flag" disallowScale="mobile" >}}
                {{</ card-scroll-gallery >}}
        </div>
    </div>
</section>

<section class="gradient even-gradient">
    <div class="main-section" id="product">
        <div class="sub-section">
            <div class="title-wrapper">
                <h2>Products</h2>
            </div>
        </div>
        <div class="css-3c0LG product-scroll-box" style="--container-max-width:1200px;">
            <div class="product-box-outer">
                <div class="product-box-wrapper">
                    <div class="swiper-slide product-box swiper-slide-next">
                        <picture>
                            <source srcset="/images/DeluxeDeckRender.png?fm=webp" type="image/webp">
                            <img src="/images/DeluxeDeckRender.png" alt="" width="400" height="433" loading="lazy">
                        </picture>
                        <div>
                            <h4 class="product-title" data-text="Standard Edition">Krieg Eterna<h4>
                        </div>
                        <div class="product-desc">
                            <p>A fast-paced strategic card game set during the Thirty Years' War. Victory on the battlefield will require deception, skill, and a little luck!
                            </p>
                        </div>
                        <div class="css-cW5DV">
                            <div class="css-nd7IL">
                                <div>
                                    <div class="css-AX10X">
                                        <a href="https://www.amazon.com/dp/B0CJHWGZYF?maas=maas_adg_3D8873ABA7D50C8B8D9E95ECC82A19D9_afap_abs&ref_=aa_maas&tag=maas"
                                            target="_blank" rel="noopener">
                                            <button class="css-lV1Vi buy-product-button css-ExOVn">
                                                Buy Now&nbsp{{< icon-link >}}
                                            </button>
                                        </a>
                <!-- Beginning of Buy With Prime Widget --
        <script async fetchpriority='high' src='https://code.buywithprime.amazon.com/bwp.v1.js'></script>
        <div
            id="amzn-buy-now"
            data-site-id="zm37qpw6y6"
            data-widget-id="w-J5EhprFtkk1J9d75bFUCK3"
            data-sku="QU-B39T-6LQM"
        ></div>
        !-- End of Buy With Prime Widget -->
                                    </div>
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
        </div>
    </div>
</section>

<section class="no-gradient">
    <div class="main-section" style="padding-bottom: 1em;">
        <div class="sub-section video-box">
            <div class="title-wrapper">
                <h4>Watch the trailer</h4>
            </div>
            {{< vid  "https://www.youtube.com/embed/yg5-dz9aM1E?si=bkm7HJy2CCnukqN-&autoplay=0&origin=http://example.com">}}
        </div>
    </div>
</section>

<section class="no-gradient">
    {{< news-letter-signup >}}
</section>