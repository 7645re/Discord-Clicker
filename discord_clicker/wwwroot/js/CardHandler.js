let row = document.getElementsByClassName("row")[0]


genCards()


async function buyPerk(evt) {
    let perkId = evt.currentTarget.getAttribute("perkId")
    let result;
    if (click > 0) {
        usermoney = await setMoneyDB(userCoefficient * click)
        await setMoneySite(usermoney["money"])
        click = 0
    }
    await asyncRequest('GET', `buyPerk?perkId=${perkId}`)
        .then(data => { result = data })
        .catch(err => console.log(err))
    if (result["result"] == "ok") {
        setMoneySite(result["userMoney"])
        userCoefficient = result["clickCoefficient"]

        let PerkPurchased = document.getElementById(perkId + "-lvl")
        let PerkPurchasedValue = PerkPurchased.children[0].innerText.replace("Lvl", "")
        let PerkPurchasedSpan = document.createElement("span")

        let PerkCost = document.getElementById(perkId + "-cost")
        let PerkCostValue = PerkCost.children[1].innerText

        PerkPurchasedSpan.innerText = "Lvl " + (parseInt(PerkPurchasedValue) + 1)
        PerkPurchased.innerHTML = ""
        PerkPurchased.appendChild(PerkPurchasedSpan)
    }
    return result
}

async function getPerksList() {
    let result
    await asyncRequest('GET', "/getPerksList")
        .then(data => { result = data })
        .catch(err => console.log(err))
    return result

}

async function genCards() {
    let result = await getPerksList()
    let perks = result["perksList"]
    let perksCount = result["perksCount"]
    for (let perk of perks) {

        row.innerHTML += `
                    <div class="col-sm-12 col-md-4">
                        <div class="item" id="perk" perkid="${perk.id}">
                            <div class="perk-img" id="${perk.id}-img"><img src="images/test.png" width="50px" style="position: relative; left: 25%"/></div>
                            <div class="perk-name" id="${perk.id}-name">${perk.name}</div>
                            <div class="perk-cost" id="${perk.id}-cost">
                                <img width="20" src="https://pnggrid.com/wp-content/uploads/2021/05/Black-and-white-Discord-Logo-1024x784.png"/>
                                <span>${perksCount[perk.id] > 0 ? perk.cost * perksCount[perk.id] : perk.cost}</span>
                            </div>
                            <div class="perk-purchased" id="${perk.id}-lvl"><span>Lvl ${perksCount[perk.id]}</span></div>
                            <div class="perk-passive" id="${perk.id}-increase">Cps +${perk.passiveCoefficient > 0 ? perk.passiveCoefficient : perk.clickCoefficient}</div>
                        </div>
                    </div>`
    }
    const perksCards = document.querySelectorAll('#perk');
    perksCards.forEach(perk => {
        perk.addEventListener('click', buyPerk, false);
    });
}




