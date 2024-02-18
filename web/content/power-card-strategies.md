---
title: "Power Card Strategies"
description: 'Krieg Eterna Power Card Strategies'
layout: single
---
<section class="gradient even-gradient">
    <div class="main-section">
    <div class="sub-section">
    <div class="title-wrapper">
        {{< outlined-title title="Power Card Strategies" h="h2" class="main-title" id="power-card-strategies">}}
       <!-- <div class="search">
            <input type="text" id="search" placeholder="Search cards by name or type">
            <button id="clear-search">
                <svg xmlns="http://www.w3.org/2000/svg" class="ionicon" viewBox="0 0 512 512">
                    <title>Backspace</title>
                    <path
                        d="M135.19 390.14a28.79 28.79 0 0021.68 9.86h246.26A29 29 0 00432 371.13V140.87A29 29 0 00403.13 112H156.87a28.84 28.84 0 00-21.67 9.84v0L46.33 256l88.86 134.11z"
                        fill="none" stroke="currentColor" stroke-linejoin="round" stroke-width="32"></path>
                    <path fill="none" stroke="currentColor" stroke-linecap="round" stroke-linejoin="round"
                        stroke-width="32"
                        d="M336.67 192.33L206.66 322.34M336.67 322.34L206.66 192.33M336.67 192.33L206.66 322.34M336.67 322.34L206.66 192.33">
                    </path>
                </svg>
            </button>
        </div>-->
    </div>
    <script>
    // @license magnet:?xt=urn:btih:5ac446d35272cc2e4e85e4325b146d0b7ca8f50c&dn=unlicense.txt Unlicense
    document.addEventListener("DOMContentLoaded", () => {
        for (e of document.getElementsByClassName("js-only")) {
            e.classList.remove("js-only");
        }
        const recipes = document.querySelectorAll("#list .rule-paragraph");
        const search = document.getElementById("search");
        //const oldheading = document.getElementById("newest-recipes");
        const clearSearch = document.getElementById("clear-search");
        const artlist = document.getElementById("list");
        search.addEventListener("input", () => {
            // grab search input value
            const searchText = search.value.toLowerCase().trim().normalize('NFD').replace(/\p{Diacritic}/gu, "");
            const searchTerms = searchText.split(" ");
            const hasFilter = searchText.length > 0;
            artlist.classList.toggle("list-searched", hasFilter);
            //oldheading.classList.toggle("hidden", hasFilter);
            // for each recipe hide all but matched
            recipes.forEach(recipe => {
                const searchString = `${recipe.textContent} ${recipe.dataset.tags}`.toLowerCase().normalize('NFD').replace(/\p{Diacritic}/gu, "");
                const isMatch = searchTerms.every(term => searchString.includes(term));
                recipe.hidden = !isMatch;
                if(!isMatch){
                    recipe.style = "display:none"
                }else{
                    recipe.style =""
                }
                recipe.classList.toggle("matched-recipe", hasFilter && isMatch);
            })
        })
        clearSearch.addEventListener("click", () => {
            search.value = "";
            recipes.forEach(recipe => {
                recipe.hidden = false;
                recipe.style = ""
                recipe.classList.remove("matched-recipe");
            })
            artlist.classList.remove("list-searched");
            oldheading.classList.remove("hidden");
        })
    })
    // @license-end
</script>
    <div class="link-list">
    <a href="#king-strategies">{{< icon-paragraph icon="King" text="King Cards" >}}</a>
    <a href="#weather-strategies">{{< icon-paragraph icon="Weather" text="Weather Cards" >}}</a>
    <a href="#jester-strategies">{{< icon-paragraph icon="Jester" text="Jester Cards" >}}</a>
    <a href="#spy-strategies">{{< icon-paragraph icon="Spy" text="Spy Cards" >}}</a>
    <a href="#hex-strategies">{{< icon-paragraph icon="Hex" text="Hex Cards" >}}</a>
    </div>
    <div class="title-wrapper">
        {{< outlined-title title="Kings" h="h3" class="rule-title" type="king" shadow="dark" id="king-strategies">}}
    </div>
        {{< strategy-description stratName="king" noImage="true">}}
        {{< strategy-description stratName="lion-king" title="Lion King" code="lion-king" imageCode="LionKing" noBase="true">}}
        {{< strategy-description stratName="sun-king" title="Sun King" code="sun-king" imageCode="SunKing" noBase="true">}}
        {{< strategy-description stratName="terror-king" title="Terror King" code="terror-king" imageCode="TerrorKing" noBase="true">}}
        {{< strategy-description stratName="traitor-king" title="Traitor King" code="traitor-king" imageCode="TraitorKing" noBase="true">}}
        {{< strategy-description stratName="winter-king" title="Winter King" code="winter-king" imageCode="WinterKing" noBase="true">}}
    <div class="title-wrapper">
        {{< outlined-title title="Weather" h="h3" class="rule-title" type="weather" shadow="light" id="weather-strategies">}}
    </div>
        {{< strategy-description stratName="clear-skies" title="Gale" imageCode="ClearSkies">}}
        {{< strategy-description stratName="weather-boon" title="Omen" imageCode="Omen">}}
        {{< strategy-description stratName="weather-bane" title="Frost" imageCode="Frost">}}
        {{< strategy-description stratName="plague" title="Plague" imageCode="Plague">}}
    <div class="title-wrapper">
        {{< outlined-title title="Jesters" h="h3" class="rule-title" type="decoy" shadow="dark" id="jester-strategies">}}
    </div>
        {{< strategy-description stratName="steal-decoy" title="Jester" imageCode="Jester">}}
        {{< strategy-description stratName="set-aside-decoy" title="Feint" imageCode="Feint">}}
        {{< strategy-description stratName="fortress" title="Fortress" imageCode="Fortress">}}
    <div class="title-wrapper">
        {{< outlined-title title="Spies" h="h3" class="rule-title" type="spy" shadow="light" id="spy-strategies">}}
    </div>
        {{< strategy-description stratName="spy" noImage="true">}}
        {{< strategy-description stratName="spy-adj" title="Saboteur" imageCode="Saboteur" noBase="true">}}
        {{< strategy-description stratName="spy-no-adj" title="Zealot" imageCode="Zealot" noBase="true">}}
        <div class="title-wrapper">
        {{< outlined-title title="Hexes" h="h3" class="rule-title" type="power" shadow="light" id="hex-strategies">}}
        </div>
        {{< strategy-description stratName="burden" title="Burden" imageCode="Burden">}}
        {{< strategy-description stratName="crusade" title="Crusade" imageCode="Crusade">}}
        {{< strategy-description stratName="death" title="Death" imageCode="Death">}}
        {{< strategy-description stratName="demise" title="Demise" imageCode="EmperorDemise">}}
        {{< strategy-description stratName="enlightenment" title="Epiphany" imageCode="Enlightenment">}}
        {{< strategy-description stratName="execution" title="Execution" imageCode="Execution">}}
        {{< strategy-description stratName="famine" title="Famine" imageCode="Famine">}}
        {{< strategy-description stratName="fate" title="Fate" imageCode="Fate">}}
        {{< strategy-description stratName="feast" title="Feast" imageCode="Feast2">}}
        {{< strategy-description stratName="grail" title="Grail" imageCode="Grail">}}
        {{< strategy-description stratName="offering" title="Offering" imageCode="Offering">}}
        {{< strategy-description stratName="redemption" title="Redemption" imageCode="Redemption">}}
        {{< strategy-description stratName="relic" title="Relic" imageCode="Relic">}}
        {{< strategy-description stratName="ruin" title="Ruin" imageCode="Ruin">}}
        {{< strategy-description stratName="styx" title="Styx" imageCode="Styx">}}
        {{< strategy-description stratName="usury" title="Usury" imageCode="Usury">}}
        {{< strategy-description stratName="void" title="Void" imageCode="Void">}}
        {{< strategy-description stratName="war" title="War" imageCode="War">}}
        {{< strategy-description stratName="wrath" title="Wrath" imageCode="Wrath">}}
</div>
</div>
</section>