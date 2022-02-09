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
        userPassiveCoefficient = result["passiveCoefficient"]
        let perkName = result["perkName"]

        let perkPurchased = document.getElementById(perkId + "-lvl")
        let perkPurchasedValue = result["buyedPerkCount"]
        let perkPurchasedSpan = document.createElement("span")

        let perkCost = document.getElementById(perkId + "-cost")
        let perkCostValue = result["perkCost"]
        let perkCostSpan = document.createElement("span")

        perkPurchasedSpan.innerText = "Lvl " + perkPurchasedValue
        perkPurchased.innerHTML = ""

        perkCostSpan.innerText = result["buyedPerkCount"] * perkCostValue
        perkCost.children[1].remove()

        perkCost.appendChild(perkCostSpan)
        perkPurchased.appendChild(perkPurchasedSpan)
        
        console.log(result)
        cpsCounter.innerHTML =  userPassiveCoefficient + " cps"
        let imgUp = '<svg xmlns="http://www.w3.org/2000/svg" width="16" height="16" fill="currentColor" class="bi bi-bag-fill" viewBox="0 0 16 16"><path d="M8 1a2.5 2.5 0 0 1 2.5 2.5V4h-5v-.5A2.5 2.5 0 0 1 8 1zm3.5 3v-.5a3.5 3.5 0 1 0-7 0V4H1v10a2 2 0 0 0 2 2h10a2 2 0 0 0 2-2V4h-3.5z"/></svg>'
        let imgDown = '<img width="16" src="images/PerksImg/NitroBoost.png"/>'
        let test = createAlert(imgUp, perkName, imgDown, `${(result["buyedPerkCount"] - 1 == 0 ? 1 : result["buyedPerkCount"] - 1)  * perkCostValue}`)
        test.style.left = "50%"
        document.body.appendChild(test)
        alertDropDown(test)
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
    console.log(perks)
    for (let perk of perks) {

        row.innerHTML += `
                    <div class="col-sm-12 col-md-4">
                        <div class="item" id="perk" perkid="${perk.id}">
                            <div class="perk-img" id="${perk.id}-img"><img src="images/PerksImg/NitroBoost.png" width="50px" style="position: relative; left: 25%"/></div>
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




