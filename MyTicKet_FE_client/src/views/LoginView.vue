<template>
  <div class="login-content">
    <div class="login-content-left">
      <div class="login-logo">
        <Transition appear @before-enter="beforeEnter" @enter="enter">
          <router-link style="display: block;" to="/">
                <img class="login-logo-img" src="https://i.ibb.co/NnfkKTT/1-1.png" alt="">
            </router-link>
        </Transition>
      </div>
    </div>
    <div class="login-content-right">
      <div class="login-form">
        <h3 class="login-form-label">Đăng nhập</h3>
        <span class="login-form-des">Đăng nhập và trải nghiệm mua vé tại MyTicket ngay hôm nay!</span>
        <b-form @submit="onSubmit" class="login-form--inner">
          <b-form-group id="email">
            <b-form-label class="input-label-login" label-for="email">Tài Khoản</b-form-label>
            <b-form-input class="form-control-login" id="email" v-model="loginForm.email" type="email"
              placeholder="Nhập tài khoản" required></b-form-input>
          </b-form-group>

          <b-form-group id="password">
            <b-form-label class="input-label-login" label-for="password">Mật Khẩu</b-form-label>
            <div class="input-password">
              <b-form-input v-if="!showPassword" class="form-control-login" :type="'password'" id="password"
                v-model="loginForm.password" placeholder="Nhập mật khẩu" required>
              </b-form-input>
              <b-form-input v-else class="form-control-login" :type="'text'" id="password"
                v-model="loginForm.password" placeholder="Nhập mật khẩu" required>
              </b-form-input>
              <b-icon v-if="!showPassword" class="show-password-btn" @click="showPassword = !showPassword"
                icon="eye"></b-icon>
                <b-icon v-else class="show-password-btn" @click="showPassword = !showPassword"
                icon="eye-slash"></b-icon>
            </div>
            <div>
              <b-form-checkbox class="checkbox-me" value="remember">
                <span>Ghi nhớ mật khẩu lần sau</span>
              </b-form-checkbox>
            </div>
          </b-form-group>

          <b-button class="login-btn" type="submit">Đăng Nhập</b-button>
          <router-link to="/signup" class="signup-btn">
            Đăng Ký
          </router-link>
        </b-form>
      </div>
      <span class="right-context-label">Bản quyền thuộc MyTicket @2022</span>
    </div>
  </div>
</template>
<script>
import { ref } from 'vue'
import gsap from 'gsap'
export default {
  setup() {
    const beforeEnter = (el) => {
      el.style.opacity = 0;
      el.style.transform = 'translateX(-100px)'
    }
    const enter = (el, done) => {
      gsap.to(el, {
        opacity: 1,
        x: 0,
        duration: 1,
        onComplete: done
      })
    }
    return { beforeEnter, enter }
  },
  data() {
    return {
      loginForm: {
        email: '',
        password: '',
        rememeber: false
      },
      showPassword: false
    }
  },
  methods: {
    onSubmit(event) {
      if(this.loginForm.email == "tanmusic0210@gmail.com"){
        if(this.loginForm.password == "02102001")
        {
          
        }else{

        }
      }
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
    }
  }
}
</script>
<style>
.login-content {
  width: 100%;
  display: flex;
  height: 100vh;
}

.login-content-right {
  width: 50%;
  background-color: #fff;
  display: flex;
  flex-direction: column;
}

.right-context-label {
  text-align: center;
  margin-bottom: 40px;
  color: var(--text-color);
}

.login-content-left {
  background-color: var(--primary-color);
  width: 50%;
  display: flex;
}

.login-form {
  width: 60%;
  margin: auto;
}

.login-logo {
  margin: auto;
  transition: opacity 1s ease-in;
}

.login-logo-img {
  width: 80%;
}

.login-form--inner {
  display: flex !important;
  flex-direction: column;
  justify-content: space-around !important;
  margin-left: 10px;
}

.login-btn {
  display: flex;
  flex-direction: column;
  align-items: center;
  padding: 8px 16px;
  gap: 10px;
  height: 40px;
  background: #e2a88c !important;
  border-radius: 8px;
  border: 1px solid #e2a88c !important;
  margin-bottom: 5px;
}

.login-btn:focus {
  outline: 0 none !important;
  outline-offset: 0 !important;
  box-shadow: 0 0 0 0.2rem #e2a88c !important;
}

.signup-btn {
  display: flex;
  flex-direction: row;
  justify-content: center;
  align-items: center;
  padding: 8px 7rem 8px 6rem;
  gap: 8px;
  height: 40px;
  background: #ffffff !important;
  border: 1px solid rgba(0, 0, 0, .1) !important;
  border-radius: 8px;
  color: #000000b3 !important;
}

.signup-btn:focus {
  outline: 0 none !important;
  outline-offset: 0 !important;
  box-shadow: 0 0 0 0.2rem #e2a88c !important;
  text-decoration: none;
}

.form-control:focus {
  color: var(--text-color) !important;
  border-color: #e2a88c !important;
  box-shadow: 0 0 0 0.1rem #e2a88c !important;
}

.form-control-login {
  border-radius: 8px !important;
}

.signup-btn:hover {
  text-decoration: none;
}

.login-form-label {
  font-weight: bold;
  margin-bottom: 20px;
}

.login-form-des {
  display: block;
  margin-bottom: 10px;
  font-weight: 500;
  opacity: 0.8;
}

.input-label-login {
  display: block;
  font-weight: bold;
  margin-bottom: 5px;
}

.checkbox-me {
  margin: 5px 0;
}

.show-password-btn {
  display: block;
  position: absolute;
  top: 10px;
  right: 10px;
  color: var(--text-color);
}

.show-password-btn svg {
  fill: currentColor;
}

.show-password-btn:hover {
  cursor: pointer;
}

.input-password {
  position: relative;
}</style>