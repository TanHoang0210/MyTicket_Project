/*!

 =========================================================
 * Vue Light Bootstrap Dashboard - v2.1.0 (Bootstrap 4)
 =========================================================

 * Product Page: http://www.creative-tim.com/product/light-bootstrap-dashboard
 * Copyright 2023 Creative Tim (http://www.creative-tim.com)
 * Licensed under MIT (https://github.com/creativetimofficial/light-bootstrap-dashboard/blob/master/LICENSE.md)

 =========================================================

 * The above copyright notice and this permission notice shall be included in all copies or substantial portions of the Software.

 */
import Vue from "vue";
import VueRouter from "vue-router";
import App from "./App.vue";
import store from './store'
// LightBootstrap plugin
import LightBootstrap from "./light-bootstrap-main";
import './data/axios'
import axios  from 'axios'
// router setup
import routes from "./routes/routes";

import "./registerServiceWorker";
// plugin setup
Vue.use(VueRouter);
Vue.use(LightBootstrap);

// configure router
const router = new VueRouter({
  routes, // short for routes: routes
  linkActiveClass: "nav-item active",
  scrollBehavior: (to) => {
    if (to.hash) {
      return { selector: to.hash };
    } else {
      return { x: 0, y: 0 };
    }
  },
});
const storedUser = sessionStorage.getItem('currentUser');
if (storedUser) {
  store.commit('setCurrentUser', JSON.parse(storedUser));
}
const token = sessionStorage.getItem('accessToken');
const retoken = sessionStorage.getItem('refreshToken');
const expiretoken = sessionStorage.getItem('tokenExpiration');
if(token && retoken && expiretoken){
  store.commit('setTokens', {
    accessToken: token,
    refreshToken: retoken,
    tokenExpiration: expiretoken,
  });
}
Vue.prototype.$fileUrl = "http://localhost:5030/myticket/";
Vue.prototype.$axios = axios;
/* eslint-disable no-new */
new Vue({
  el: "#app",
  render: (h) => h(App),
  router,
});
