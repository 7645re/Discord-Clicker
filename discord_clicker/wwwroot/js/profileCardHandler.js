async function setDataToProfileCard() {
    // let builds = JSON.parse(localStorage.getItem("builds"))
    // let buildsCount = JSON.parse(localStorage.getItem("buildsCount"))
    // let profileBuildsHTML = document.getElementsByClassName("builds")[0]
    let profileNicknameHTML = document.getElementsByClassName("nickname")[0]
    let profileHeaderHTML = document.getElementsByClassName("modal-header")[0]
    // for (var build in builds) {
    //     if (buildsCount[builds[build].id]) {
    //         profileBuildsHTML.innerHTML += `<img class="profile-badge" src="images/BuildsImg/${builds[build].name}.svg"/>`
    //     }
    // }
    console.log(profileNicknameHTML)
    profileNicknameHTML.textContent = lUserNickname + "#" + lUserId
}