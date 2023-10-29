<template>
    <div class="header">
        <div class="header-inner"> 
            <div class="language-control">
                <div class="language-control--item">
                    <img class="language-type"
                        :src="currentLocalization.flagImg"
                        alt="">
                        <span class="language-name ">{{currentLocalization.name.toUpperCase() }}</span>
                        <div class="language-choice" > 
                            <div class="language-choice--item"
                            v-for="(localData,index) in localizations"
                            :id="index"
                            :key="localData"
                            v-on:click="myLanguage(localData,index)"
                              v-bind:class="{ active: index== isActive }">
                                <img class="language-type language-type-item"
                                 :src="localData.flagImg" alt="">
                                <span class="language-name-choice">{{ localData.name.toUpperCase() }}</span>
                            </div>
                        </div>
                </div>
            </div>
        <div class="left-header">
            <router-link style="text-decoration: none; color: inherit;" class="left-header--logo" to="/">
                <img src="https://i.ibb.co/VQGNx7z/Logo.png" alt="">
            </router-link>
            <div class="left-header--control">
                <b-nav class="left-header--list">
                    <b-nav-item class="left-header--item">
                        <router-link style="text-decoration: none;" to="/event">
                            Sự kiện
                        </router-link>
                    </b-nav-item>
                    <b-nav-item class="left-header--item">
                        <router-link style="text-decoration: none;" to="/event">
                            Thể loại
                        </router-link>
                    </b-nav-item>
                    <b-nav-item class="left-header--item">
                        <router-link style="text-decoration: none;" to="/event">
                            Sân vận động
                        </router-link>
                    </b-nav-item>
                    <b-nav-item class="left-header--item">
                        <router-link style="text-decoration: none;" to="/event">
                            Về chúng tôi
                        </router-link>
                    </b-nav-item>
                </b-nav>
            </div>
            <div class="header-search">
                <div class="header-search--item">
                    <router-link class="header-search--link" to="/about">
                        <button class="header-search--button">
                            <svg xmlns="http://www.w3.org/2000/svg" class="header-search--icon" height="1em"
                                viewBox="0 0 512 512"><!--! Font Awesome Free 6.4.0 by @fontawesome - https://fontawesome.com License - https://fontawesome.com/license (Commercial License) Copyright 2023 Fonticons, Inc. -->
                                <path
                                    d="M416 208c0 45.9-14.9 88.3-40 122.7L502.6 457.4c12.5 12.5 12.5 32.8 0 45.3s-32.8 12.5-45.3 0L330.7 376c-34.4 25.2-76.8 40-122.7 40C93.1 416 0 322.9 0 208S93.1 0 208 0S416 93.1 416 208zM208 352a144 144 0 1 0 0-288 144 144 0 1 0 0 288z" />
                            </svg>
                        </button>
                    </router-link>
                    <b-form-input class="header-search--input" v-model="text" placeholder="Tìm Kiếm Sự kiện"></b-form-input>
                </div>
            </div>
            <div class="user-control">
                <router-link v-if="isLogin" class="register-link" style="text-decoration: none; color: #555;" to="/login">
                    <b-icon style="margin-right: 5px;" icon="person-fill"></b-icon>
                    Đăng Nhập
                </router-link>
                <div v-else class="user-info">
                    <b-icon style="margin-right: 5px;" icon="person-fill"></b-icon>
                    {{ user.userName }}
                    <div class="user-action">
                        <ul class="user-action-list">
                            <li class="user-action-item">
                                <router-link class="user-action-link" style="text-decoration: none;" to="/event">
                            Thông tin tài khoản
                                </router-link>
                            </li>
                            <li class="user-action-item">
                                <router-link class="user-action-link" style="text-decoration: none;" to="/event">
                            Vé của bạn
                                </router-link>
                            </li>
                            <li class="user-action-item">
                                <router-link class="user-action-link" style="text-decoration: none;" to="/event">
                            Vé chuyển nhượng
                                </router-link>
                            </li>
                            <li class="user-action-item">
                                <router-link class="user-action-link" style="text-decoration: none;" to="/event">
                            Vé trả
                                </router-link>
                            </li>
                            <li class="user-action-item">
                                <router-link class="user-action-link" style="text-decoration: none;" to="/event">
                            Đăng xuất
                                </router-link>
                            </li>
                        </ul>
                    </div>
                </div>
            </div>
        </div>
    </div>
    </div>
</template>
  
<script>
export default {
    name: 'Header',
    props:['isLogin'],
    data: () => {
        return {
            currentLocalization:{
                id:0,
                name:"vi",
                flagImg:"data:image/svg+xml;base64,PHN2ZyB3aWR0aD0iMjQiIGhlaWdodD0iMjQiIHZpZXdCb3g9IjAgMCAyNCAyNCIgZmlsbD0ibm9uZSIgeG1sbnM9Imh0dHA6Ly93d3cudzMub3JnLzIwMDAvc3ZnIj4KPHBhdGggZD0iTTI0IDIwQzI0IDIwLjU1MjUgMjMuNTUyNSAyMSAyMyAyMUgxQzAuNDQ3NSAyMSAwIDIwLjU1MjUgMCAyMFY0QzAgMy40NDc1IDAuNDQ3NSAzIDEgM0gyM0MyMy41NTI1IDMgMjQgMy40NDc1IDI0IDRWMjBaIiBmaWxsPSIjRDgyODI3Ii8+CjxwYXRoIGQ9Ik0xNi4xODAzIDEwLjUyMDdIMTMuMjM0OEwxMS45OTk4IDcuODQyMTZMMTAuNzYzMyAxMC41MjA3SDcuODE4ODVMOS45NzAzNSAxMi43NDcyTDkuMTc1MzUgMTUuOTczN0wxMS45OTk4IDE0LjM2OTJMMTQuODIzOCAxNS45NzM3TDE0LjAyODggMTIuNzQ3MkwxNi4xODAzIDEwLjUyMDdaIiBmaWxsPSIjRkZGRDM4Ii8+Cjwvc3ZnPgo=",
            },
            localizations:[
                {
                    id:1,
                    name:"vi",
                    flagImg:"data:image/svg+xml;base64,PHN2ZyB3aWR0aD0iMjQiIGhlaWdodD0iMjQiIHZpZXdCb3g9IjAgMCAyNCAyNCIgZmlsbD0ibm9uZSIgeG1sbnM9Imh0dHA6Ly93d3cudzMub3JnLzIwMDAvc3ZnIj4KPHBhdGggZD0iTTI0IDIwQzI0IDIwLjU1MjUgMjMuNTUyNSAyMSAyMyAyMUgxQzAuNDQ3NSAyMSAwIDIwLjU1MjUgMCAyMFY0QzAgMy40NDc1IDAuNDQ3NSAzIDEgM0gyM0MyMy41NTI1IDMgMjQgMy40NDc1IDI0IDRWMjBaIiBmaWxsPSIjRDgyODI3Ii8+CjxwYXRoIGQ9Ik0xNi4xODAzIDEwLjUyMDdIMTMuMjM0OEwxMS45OTk4IDcuODQyMTZMMTAuNzYzMyAxMC41MjA3SDcuODE4ODVMOS45NzAzNSAxMi43NDcyTDkuMTc1MzUgMTUuOTczN0wxMS45OTk4IDE0LjM2OTJMMTQuODIzOCAxNS45NzM3TDE0LjAyODggMTIuNzQ3MkwxNi4xODAzIDEwLjUyMDdaIiBmaWxsPSIjRkZGRDM4Ii8+Cjwvc3ZnPgo=",

                },
                {
                    id:2,
                    name:"en",
                    flagImg:"data:image/svg+xml;base64,PD94bWwgdmVyc2lvbj0iMS4wIiBlbmNvZGluZz0iVVRGLTgiIHN0YW5kYWxvbmU9Im5vIj8+DQo8IS0tIENyZWF0ZWQgd2l0aCBTb2RpcG9kaSAoImh0dHA6Ly93d3cuc29kaXBvZGkuY29tLyIpIC0tPg0KPCEtLSAvQ3JlYXRpdmUgQ29tbW9ucyBQdWJsaWMgRG9tYWluIC0tPg0KPCEtLQ0KDQo8cmRmOlJERiB4bWxucz0iaHR0cDovL3dlYi5yZXNvdXJjZS5vcmcvY2MvIg0KICAgICB4bWxuczpkYz0iaHR0cDovL3B1cmwub3JnL2RjL2VsZW1lbnRzLzEuMS8iDQogICAgIHhtbG5zOnJkZj0iaHR0cDovL3d3dy53My5vcmcvMTk5OS8wMi8yMi1yZGYtc3ludGF4LW5zIyI+DQo8V29yayByZGY6YWJvdXQ9IiI+DQogICAgPGRjOnRpdGxlPk5ldyBaZWFsYW5kLCBBdXN0cmFsaWEsIFVuaXRlZCBLaW5nZG9tLCBVbml0ZWQgU3RhdGVzLCANCkJvc25pYSBhbmQgSGVyemVnb3ZpbmEsIEF6ZXJiYWlqYW4sIEFybWVuaWEsIEJhaGFtYXMsIEJlbGdpdW0sIEJlbmluLCANCkJ1bGdhcmlhLCBFc3RvbmlhLCBGaW5sYW5kLCBHYWJvbiwgR2FtYmlhLCBHZXJtYW55LCBHcmVlY2UsIEdyZWVubGFuZCwgDQpHdWluZWEsIEhvbmR1cmFzLCBJc3JhZWwsIEphbWFpY2EsIEpvcmRhbiwgYW5kIFJvbWFuaWEgRmxhZ3M8L2RjOnRpdGxlPg0KICAgIDxkYzpyaWdodHM+PEFnZW50Pg0KICAgICAgIDxkYzp0aXRsZT5EYW5pZWwgTWNSYWU8L2RjOnRpdGxlPg0KICAgIDwvQWdlbnQ+PC9kYzpyaWdodHM+DQogICAgPGxpY2Vuc2UgcmRmOnJlc291cmNlPSJodHRwOi8vd2ViLnJlc291cmNlLm9yZy9jYy9QdWJsaWNEb21haW4iIC8+DQo8L1dvcms+DQoNCjxMaWNlbnNlIHJkZjphYm91dD0iaHR0cDovL3dlYi5yZXNvdXJjZS5vcmcvY2MvUHVibGljRG9tYWluIj4NCiAgICA8cGVybWl0cyByZGY6cmVzb3VyY2U9Imh0dHA6Ly93ZWIucmVzb3VyY2Uub3JnL2NjL1JlcHJvZHVjdGlvbiIgLz4NCiAgICA8cGVybWl0cyByZGY6cmVzb3VyY2U9Imh0dHA6Ly93ZWIucmVzb3VyY2Uub3JnL2NjL0Rpc3RyaWJ1dGlvbiIgLz4NCiAgICA8cGVybWl0cyByZGY6cmVzb3VyY2U9Imh0dHA6Ly93ZWIucmVzb3VyY2Uub3JnL2NjL0Rlcml2YXRpdmVXb3JrcyIgLz4NCjwvTGljZW5zZT4NCjwvcmRmOlJERj4NCi0tPg0KPHN2ZyBpZD0ic3ZnMSIgeG1sbnM6cmRmPSJodHRwOi8vd3d3LnczLm9yZy8xOTk5LzAyLzIyLXJkZi1zeW50YXgtbnMjIiB4bWxucz0iaHR0cDovL3d3dy53My5vcmcvMjAwMC9zdmciIGhlaWdodD0iNDgwIiB3aWR0aD0iNjQwIiB2ZXJzaW9uPSIxLjEiIHhtbG5zOmNjPSJodHRwOi8vY3JlYXRpdmVjb21tb25zLm9yZy9ucyMiIHhtbG5zOmRjPSJodHRwOi8vcHVybC5vcmcvZGMvZWxlbWVudHMvMS4xLyI+DQogPG1ldGFkYXRhIGlkPSJtZXRhZGF0YTI5OTUiPg0KICA8cmRmOlJERj4NCiAgIDxjYzpXb3JrIHJkZjphYm91dD0iIj4NCiAgICA8ZGM6Zm9ybWF0PmltYWdlL3N2Zyt4bWw8L2RjOmZvcm1hdD4NCiAgICA8ZGM6dHlwZSByZGY6cmVzb3VyY2U9Imh0dHA6Ly9wdXJsLm9yZy9kYy9kY21pdHlwZS9TdGlsbEltYWdlIi8+DQogICA8L2NjOldvcms+DQogIDwvcmRmOlJERj4NCiA8L21ldGFkYXRhPg0KIDxkZWZzIGlkPSJkZWZzMyI+DQogIDxjbGlwUGF0aCBpZD0iY2xpcFBhdGg4NjczIiBjbGlwUGF0aFVuaXRzPSJ1c2VyU3BhY2VPblVzZSI+DQogICA8cmVjdCBpZD0icmVjdDg2NzUiIGZpbGwtb3BhY2l0eT0iMC42NyIgaGVpZ2h0PSI1MTIiIHdpZHRoPSI2ODIuNjciIHk9Ii0uMDAwMDAyODYxMyIgeD0iLTg1LjMzMyIvPg0KICA8L2NsaXBQYXRoPg0KIDwvZGVmcz4NCiA8ZyBpZD0iZmxhZyIgY2xpcC1wYXRoPSJ1cmwoI2NsaXBQYXRoODY3MykiIHRyYW5zZm9ybT0ibWF0cml4KC45Mzc1MCAwIDAgLjkzNzUwIDgwIC4wMDAwMDI2ODI1KSI+DQogIDxnIGlkPSJnNTc4IiBzdHJva2Utd2lkdGg9IjFwdCIgdHJhbnNmb3JtPSJtYXRyaXgoMTcuMDY3IDAgMCAxNy4wNjcgLTI1NiAtMC4wMDAwMDI0KSI+DQogICA8cmVjdCBpZD0icmVjdDEyNCIgaGVpZ2h0PSIzMCIgd2lkdGg9IjYwIiB5PSIwIiB4PSIwIiBmaWxsPSIjMDA2Ii8+DQogICA8ZyBpZD0iZzU4NCI+DQogICAgPHBhdGggaWQ9InBhdGgxNDYiIGQ9Im0wIDB2My4zNTQxbDUzLjI5MiAyNi42NDZoNi43MDh2LTMuMzU0bC01My4yOTItMjYuNjQ2aC02LjcwOHptNjAgMHYzLjM1NGwtNTMuMjkyIDI2LjY0NmgtNi43MDh2LTMuMzU0bDUzLjI5Mi0yNi42NDZoNi43MDh6IiBmaWxsPSIjZmZmIi8+DQogICAgPHBhdGggaWQ9InBhdGgxMzYiIGQ9Im0yNSAwdjMwaDEwdi0zMGgtMTB6bS0yNSAxMHYxMGg2MHYtMTBoLTYweiIgZmlsbD0iI2ZmZiIvPg0KICAgIDxwYXRoIGlkPSJwYXRoMTQxIiBkPSJtMCAxMnY2aDYwdi02aC02MHptMjctMTJ2MzBoNnYtMzBoLTZ6IiBmaWxsPSIjYzAwIi8+DQogICAgPHBhdGggaWQ9InBhdGgxNTAiIGQ9Im0wIDMwIDIwLTEwaDQuNDcybC0yMCAxMGgtNC40NzJ6bTAtMzAgMjAgMTBoLTQuNDcybC0xNS41MjgtNy43NjM5di0yLjIzNjF6bTM1LjUyOCAxMCAyMC0xMGg0LjQ3MmwtMjAgMTBoLTQuNDcyem0yNC40NzIgMjAtMjAtMTBoNC40NzJsMTUuNTI4IDcuNzY0djIuMjM2eiIgZmlsbD0iI2MwMCIvPg0KICAgPC9nPg0KICA8L2c+DQogPC9nPg0KPC9zdmc+DQo=",

                }
            ],
            isActive: false,
            user:{
                userId:1,
                userName:"Nhat Tan",
            },
        }
    },
    methods: {
        myLanguage: function (language) {
            this.isActive = !this.isActive;
             this.currentLocalization = language;
            // some code to filter users
        }
    }
}
</script>
<style scoped lang="scss">
.header{
    display: flex;
    flex-direction: row;
}
.header-inner{
    display: flex;
    -webkit-box-align: center;
    align-items: center;
    z-index: 2;
    width: 100%;
    background-color: var(--primary-color);
    justify-content: space-evenly;
    flex-direction: column;
    top: 0px;
}

.left-header {
    margin: auto;
    display: flex;
    -webkit-box-align: center;
    align-items: center;
    height: 100%;
    padding: 10px 0;
    position: relative;
}

.left-header--logo {
    text-decoration: none;
    display: flex;
    -webkit-box-align: center;
    align-items: center;
    margin-right: 20px;
    height: 100%;
}
.category-item{
    line-height: 1em !important;
    font-size: 2rem !important;
}
.list-group-item {
    padding: 0 !important;
}

.categoy-item {
    background-color: #fff;
}

.left-header--logo img {
    width: 120px;
}

.left-header--control {
    height: 100%;
    display: flex;
    align-items: center;
    flex-wrap: nowrap;
    position: relative;
}

.left-header--control ul {
    height: 100%;
}

.left-header--item {
    display: flex;
    height: 100%;
    text-align: center;
    align-items: center;
}

.left-header--item a {
    height: 100%;
    width: 100%;
    display: flex;
    align-items: center;
    color: var(--text-color);
    font-size: 1.2rem;
    font-weight: 600;
    line-height: 50px;
    transition: all ease-in 0.2s;
}
.left-header--item a:hover {
    color: var(--primary-color-bold);
    -webkit-transform: translateY(-10px);
}

.header-search {
    display: flex;
    -webkit-box-align: center;
    align-items: center;
    position: relative;
    width: 372px;
}

.header-search--item {
    display: flex;
    font-size: 16px;
    -webkit-box-align: center;
    align-items: center;
    position: relative;
    width: 100%;
    color: var(--text-color);
}

.header-search--item svg {
    fill: currentColor;
}

.header-search--input {
    background-color: var(--primary-color);
    line-height: 24px;
    height: 44px;
    width: 80%;
    outline: none;
    border: 1px solid transparent;
    border-radius: 8px 0px 0px 8px;
    box-shadow: none;
    border: 1px solid var(--primary-color-bold) !important;
    transition: 0.3s;
    border-right: none !important;
}

.header-search--link {
    position: absolute;
    right: 0;
    height: 100%;
    width: 20%;
    display: flex;
    background-color: var(--primary-color);
    border: 1px solid var(--primary-color-bold);
    border-left: none;
    border-radius: 0px 8px 8px 0px;
}

.header-search--button:hover {
    background-color: var(--primary-color-bold);
    color: #fff;
}

.header-search--button {
    width: 100%;
    border: none;
    height: 100%;
    text-align: center;
    margin: auto;
    color: var(--primary-color-bold);
    background-color: var(--primary-color);
    transition: all 0.2s;
    border-radius: 0px 7px 7px 0px;
}

.header-search--icon {
    width: 16px;
    height: 16px;
}

input::placeholder {
    color: var(--primary-color-bold) !important;
    opacity: 1;
}

input:focus::placeholder {
    color: var(--primary-color-bold) !important;
    opacity: 1;
}

input:focus {
    box-shadow: 10px 10px;
}

.header-search--icon:hover {
    cursor: pointer;
}

.form-control:focus {
    box-shadow: none !important;
}

//rigth-header
.user-control {
    display: flex;
    align-items: center;
    height: 100%;
    background-color: var(--primary-color);
    position: relative;
}

.right-header {
    display: flex;
    align-items: center;
    margin-left: auto;
    margin-right: 0px;
    height: 100%;
}

.register-link {
    border-radius: 25px 25px;
    border: 1px solid var(--primary-color-bold);
    display: flex;
    align-items: center;
    font-style: normal;
    font-weight: normal;
    font-size: 1rem;
    line-height: 45px;
    padding-left: 20px;
    padding-right: 20px;
    margin-left: 20px;
    color: var(--primary-color-bold) !important;
    transition: background-color ease-in 0.1s;
}
.user-info{
    display: flex;
    align-items: center;
    font-style: normal;
    font-weight: normal;
    font-size: 1.2rem;
    line-height: 45px;
    padding-left: 20px;
    padding-right: 20px;
    margin-left: 20px;
    color: var(--primary-color-bold);
    transition: all ease-in 0.1s;
    cursor: pointer;
}
.user-info:hover{
    color: var(--primary-color-hover-bold);
}
.user-info:hover .user-action-list{
    display: block;
    opacity: 1;
}
.user-action-list::before {
    content: "";
    display: block;
    width: 0;
    height: 0;
    border-right: 12px solid transparent;
    border-left: 12px solid transparent;
    border-top: 12px solid transparent;
    border-bottom: 12px solid var(--primary-color-bold);
    top: -24px;
    position: absolute;
    right: calc(45%);
}
.register-link:hover {
    background-color: var(--primary-color-bold);
    color: #fff !important;
}
.user-action{
    position: absolute;
    background-color: #fff;
    width: 170px;
    top: 90%;
    font-size: 16px;
}
.user-action-list{
    opacity: 0;
    list-style: none;
    padding: 0 5px;
    margin: 0;
    background-color: #fff;
    position: absolute;
    top: 90%;
    width: 200px;
    right: 0;
    display: none;
    font-size: 1rem;
    color: var(--primary-color-bold) !important;
    z-index: 1;
    border: 1px solid var(--primary-color-bold);
}
.user-action-item{
    text-align: center;
    border-top: 1px solid var(--primary-color-bold);
}
.user-action-item:first-of-type{
    border-top:none ;
}
.user-action-item a{
    color: var(--primary-color-bold);
}
.user-action-item a:hover{
    color: var(--primary-color-hover-bold);
    cursor: pointer;
}
.language-control {
    margin: auto;
    display: flex;
    -webkit-box-align: center;
    align-items: center;
    background-color: var(--primary-color-bold);
    position: relative;
    padding: 5px 0;
    width: 100%;
}

.language-control--item:hover {
    cursor: pointer;
}
.language-name{
    text-align: center;
    margin-left: 10px;
    color: #fff;
    font-weight: bold;
    font-size: 1.2rem;
    align-items: center;
}
.language-name-choice{
    margin-left: 5px;
    color: var(--primary-color-hover-bold);
    font-size: 1.2rem;
}
.language-control--item:hover .language-choice {
    display: block;
}
.language-control--item {
    display: flex;
    margin-left: 90%;
}

.language-type {
    margin-left: 8px;
    width: 24px;
    border-radius: 50%;
}

.dropdown-img {
    margin-left: 8px;
    width: 8px;
}

.language-type-item {
    margin-right: 12px;
}


.language-choice {
    list-style: none;
    padding: 0 5px;
    margin: 0;
    background-color: #fff;
    position: absolute;
    top: 100%;
    width: 100px;
    right: 3%;
    display: none;
    font-size: 1rem;
    color: var(--primary-color-bold) !important;
    z-index: 1;
    border: 1px solid var(--primary-color-bold);
    border-radius: 4px 4px;
}
.language-choice::before{
    content: "";
    display: block;
    width: 0;
    height: 0;
    border-right: 12px solid transparent;
    border-left: 12px solid transparent;
    border-top: 12px solid transparent;
    border-bottom: 12px solid #fff;
    top: -24px;
    position: absolute;
    right: calc(45%);
}
.language-choice--item {
    display: flex;
    -webkit-box-align: center;
    align-items: center;
    padding: 8px;
    border-top: 1px solid var(--primary-color-bold);
    cursor: pointer;
}
.language-choice--item:first-of-type{
    border-top:none ;
}
.active {
    font-weight: bold;
}

@media only screen and (max-width: 768px) {
    .right-header {
        display: none;
    }

    .left-header {
        width: 100%;
    }

    .header {
        height: 65px;
    }
}</style>