<template>
    <div class="content">
        <div class="container-fluid">
            <div class="row container-inner"
                :style="{ display: 'flex', height: '100vh', backgroundImage: 'url(' + $fileUrl + bannerImage + ')' }">
                <div class="col-md-7">

                </div>
                <div class="col-md-4">
                    <card class="login" style="margin-top: 40% !important;">
                        <form>
                            <div class="row">
                                <div class="col-md-12" style="display: flex; flex-direction: column;">
                                    <div>
                                        <h3 style="margin: auto;" slot="header" class="card-title col-md-6">Đăng Nhập</h3>
                                        <base-input type="text" label="Tên đăng nhập" placeholder="Tên đăng nhập"
                                            v-model="loginForm.username">
                                        </base-input>
                                        <base-input type="password" label="Mật khẩu" placeholder="Mật Khẩu"
                                            v-model="loginForm.password">
                                        </base-input>
                                    </div>
                                    <div>
                                        <button type="submit" class="btn-login btn btn-info btn-fill float-right "
                                            @click="onSubmit">
                                            Đăng Nhập
                                        </button>
                                    </div>
                                </div>
                            </div>
                        </form>
                    </card>
                </div>
            </div>
        </div>
    </div>
</template>
<script>
import { getCurrentUser, Login, refreshAccessToken } from '@/service/auth/authService'
import store from '@/store'
import Card from 'src/components/Cards/Card.vue'
export default {
    components: {
        Card
    },
    data() {
        return {
            bannerImage: null,
            loginForm: {
                username: '',
                password: '',
                rememeber: false
            },
            showPassword: false,
            refreshIntervalId: null,
        }
    },
    notifyVue(type, message, verticalAlign, horizontalAlign, color) {
            this.$notifications.notify(
                {
                    message: `<span><b>${type}</b> - ${message}</span>`,
                    icon: 'nc-icon nc-app',
                    horizontalAlign: horizontalAlign,
                    verticalAlign: verticalAlign,
                    type: color
                })
        },
    methods: {
        async handleSuccessfulLogin(tokenExpiration) {
            // Thời gian giữa mỗi lần làm mới token (1 giờ)
            const refreshInterval = (tokenExpiration - 5 * 60) * 1000;
            console.log("Thời gian giữa mỗi lần làm mới: " + refreshInterval);

            // Đảm bảo rằng interval không tồn tại trước khi tạo mới
            if (this.refreshIntervalId) {
                clearInterval(this.refreshIntervalId);
            }

            // Bắt đầu làm mới tự động bằng cách sử dụng setInterval
            this.refreshIntervalId = setInterval(() => {
                // Thực hiện hàm refresh token ở đây
                refreshAccessToken(store.getters.refreshToken);
            }, refreshInterval);
        },

        // Hàm này có thể được gọi khi bạn muốn dừng tự động làm mới (ví dụ: khi người dùng đăng xuất)
        stopAutoRefresh() {
            if (this.refreshIntervalId) {
                clearInterval(this.refreshIntervalId);
            }
        },
        async onSubmit(event) {
            event.preventDefault()
            await this.onLogin();
            sessionStorage.setItem("accessToken", store.getters.accessToken)
            sessionStorage.setItem("refreshToken", store.getters.refreshToken)
            sessionStorage.setItem("tokenExpiration", store.getters.tokenExpiration)
            sessionStorage.setItem("currentUser", JSON.stringify(store.getters.currentUser))
            console.log(store.getters.currentUser)
        },
        onReset(event) {
            event.preventDefault()
            // Reset our form values
            this.loginForm.email = ''
            this.loginForm.password = ''
        },
        showPassword() {
            if (this.inputtype === "text") {
                this.inputtype = "password"
            }
            if (this.inputtype === "password") {
                this.inputtype = "text"
            }
        },
        async onLogin() {
            const res = await Login(this.loginForm.username, this.loginForm.password)

            if (res.status !== 200) {
                    this.notifyVue('Thất bại', res.response.data.error_description, 'top', 'right', 'danger')
            } else {
                this.notifyVue('Thành công','Đăng nhập thành công', 'top', 'right', 'success')
                await this.handleSuccessfulLogin(store.getters.tokenExpiration)
                await getCurrentUser()
                const routeInfo = this.$route.query.routeInfo;
                if (routeInfo) {
                    this.$router.push(routeInfo);
                } else {
                    this.$router.push("/");
                }
            }
        },
        notifyVue(type, message, verticalAlign, horizontalAlign, color) {
            this.$notifications.notify(
                {
                    message: `<span><b>${type}</b> - ${message}</span>`,
                    icon: 'nc-icon nc-app',
                    horizontalAlign: horizontalAlign,
                    verticalAlign: verticalAlign,
                    type: color
                })
        },
    },
    mounted() {
        this.bannerImage = 'api/file/get?folder=&file=bannerAdmin.webp';
    }
}
</script>
<style>
.card.login{
    background-color: transparent !important;
    color: #fff !important;
}

.btn-login {
    background-color: #830158 !important;
    border: none;
}

.container-inner {
    background-repeat: no-repeat;
    background-size: cover;
    /* Optional: Adjusts the size of the background image to cover the entire container */
    height: 100vh;
}</style>
