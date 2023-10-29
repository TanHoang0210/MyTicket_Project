<template>
    <footer class="footer">
        <div class="footer-content grid">
            <div class="footer-inner row">
                <div class="footer-connect-list col-lg-3 col-md-3 col-sm-6 col-xs-12 col-12">
                    <router-link style="text-decoration: none; color: inherit;" class="" to="/">
                <img class="logo-img" src="https://i.ibb.co/Xyk1fHX/logo-pink-textcolor.png" alt="">
            </router-link>
                <h5>Kết nối với chúng tôi</h5>
                <ul class="footer-connect-item">
                    <li class="connect-item">
                        <b-icon style="margin-right: 5px;" icon="facebook"></b-icon>
                    </li>
                    <li class="connect-item">
                        <b-icon style="margin-right: 5px;" icon="instagram"></b-icon>
                    </li>
                    <li class="connect-item">
                        <b-icon style="margin-right: 5px;" icon="twitter"></b-icon>
                    </li>
                </ul>
                </div>
                <div class="footer-action col-lg-9 col-md-9 col-sm-6 col-xs-12 col-12">
                    <div class="row">
                    <div class="footer-action-list col-lg-3 col-md-3 col-sm-12 col-xs-12 col-12 collapseBox">
                    <span class="footer-title">
                        Thể Loại
                    </span>
                    <ul class="footer-action-item">
                        <li class="action-item" v-for="category in categories">
                            <router-link to="/about">
                                {{category.name}}
                            </router-link>
                        </li>
                    </ul>
                    </div>
                    <div class="footer-action-list col-lg-3 col-md-3 col-sm-12 col-xs-12 col-12 collapseBox">
                    <span class="footer-title">
                        Chăm sóc khách hàng
                    </span>
                    <ul class="footer-action-item">
                        <li class="action-item">
                            <router-link to="/about">
                                Câu hỏi thường gặp
                            </router-link>
                        </li>
                        <li class="action-item">
                            <router-link to="/about">
                               Liên hệ với chúng tôi
                            </router-link>
                        </li>
                        <li class="action-item">
                            <router-link to="/about">
                                Điều khoản sử dụng
                            </router-link>
                        </li>
                        <li class="action-item">
                            <router-link to="/about">
                                Tin tức
                            </router-link>
                        </li>
                    </ul>
                    </div>
                    <div class="footer-action-list col-lg-3 col-md-3 col-sm-12 col-xs-12 col-12 collapseBox">
                    <span class="footer-title">
                        Cùng bán vé nào!
                    </span>
                    <ul class="footer-action-item">
                        <li class="action-item">
                            <router-link to="/about">
                                Bán vé cho sự kiện của bạn
                            </router-link>
                        </li>
                    </ul>
                    </div>
                    <div class="footer-action-list col-lg-3 col-md-3 col-sm-12 col-xs-12 col-12 collapseBox">
                    <span class="footer-title">
                        My Ticket
                    </span>
                    <ul class="footer-action-item">
                        <li class="action-item">
                            <router-link to="/about">
                                Về chúng tôi
                            </router-link>
                        </li>
                        <li class="action-item">
                            <router-link to="/about">
                                Tuyển dụng
                            </router-link>
                        </li>
                        <li class="action-item">
                            <router-link to="/about">
                                Toàn cầu
                            </router-link>
                        </li>
                    </ul>
                    </div>
                </div>
                </div>
            </div>
        </div>
        <span class="footer-description">
            @ 2001-2023 My Ticket. All rights reserved.
        </span>
    </footer>
</template>
<script>
export default{
    name:'HomeFooter',
    props:['categories']
}
</script>
<style>
.footer{
    background-color: var(--primary-color-bold);
    color: var(--text-color);
    display: flex;
    flex-direction: column;
    padding: 50px 0 20px 0;
    bottom: 0;
}
.logo-img{
    width: 150px;
    margin-bottom: 10px;
}
.footer-inner{
    width: 80%;
    margin: auto !important;
}
.footer-content{
    display: flex;
}
.footer-title{
    align-items: center;
    font-weight: 600;
}
.footer-action-list{
    display: flex;
    align-items: center;
    flex-direction: column;
}
.footer-action-item{
    list-style: none;
    padding-left: 0;
}
.action-item{
    padding: 5px 0;
}
.footer-connect-item{
    display: flex;
    list-style: none;
    padding-top: 10px;
    padding-left: 0;
    margin: auto;
}
.footer-connect-list{
    font-size: 1.6rem;
    display: flex;
    flex-direction: column;
    justify-content: center;
}
.footer-connect-list h5{
    font-size: 1.4rem;
    margin: auto;
}
.footer-connect-list a{
    margin: auto;
}
.connect-item{
    margin-right: 10px;
    align-items: center;
}
.action-item a{
    color: var(--text-color) !important;
}
.footer-description{
    padding-top: 50px;
    margin: auto;
}
.footer-action-list{
    font-size: 1.2rem;
}
</style>