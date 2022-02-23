function setDataToProfileCard() {
    let profileNicknameHTML = document.getElementsByClassName("nickname")[0]
    let profilePerksHTML = document.getElementsByClassName("perks")[0]
    let profileHeaderHTML = document.getElementsByClassName("modal-header")[0]
    let perks = JSON.parse(localStorage.getItem("perks"))
    let perksCount = JSON.parse(localStorage.getItem("perksCount"))
    for (var perk in perks) {
        if (perksCount[perks[perk].id]) {
            profilePerksHTML.innerHTML += `<img class="profile-badge" src="images/PerksImg/${perks[perk].name}.svg"/>`
        }
    }
    console.log(perksCount)
    profileNicknameHTML.textContent = lUserNickname + "#" + lUserId



}