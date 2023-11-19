import Vue from 'vue'
import App from './App.vue'
import './registerServiceWorker'
import router from './router'
import store from './store'
import { BootstrapVue, IconsPlugin,BootstrapVueIcons } from 'bootstrap-vue'
/* import the fontawesome core */
import { library } from '@fortawesome/fontawesome-svg-core'

/* import font awesome icon component */
import { FontAwesomeIcon } from '@fortawesome/vue-fontawesome'
import { Carousel3d} from 'vue-carousel-3d'
/* import specific icons */
import { faUserSecret } from '@fortawesome/free-solid-svg-icons'
import 'bootstrap/dist/css/bootstrap.css'
import Toasted from 'vue-toasted';
import 'bootstrap-vue/dist/bootstrap-vue.css'
import VueSmoothScroll from 'vue2-smooth-scroll'
import { BSpinner } from 'bootstrap-vue'
import './data/axios'
import axios  from 'axios'
/* add icons to the library */
library.add(faUserSecret)
Vue.prototype.$fileUrl = "http://localhost:5030/myticket/";
/* add font awesome icon component */
Vue.component('font-awesome-icon', FontAwesomeIcon)
Vue.component('b-spinner', BSpinner)
const storedUser = localStorage.getItem('currentUser');
if (storedUser) {
  store.commit('setCurrentUser', JSON.parse(storedUser));
}
const token = localStorage.getItem('accessToken');
const retoken = localStorage.getItem('refreshToken');
const expiretoken = localStorage.getItem('tokenExpiration');
if(token && retoken && expiretoken){
  store.commit('setTokens', {
    accessToken: token,
    refreshToken: retoken,
    tokenExpiration: expiretoken,
  });
}
Vue.config.productionTip = false
Vue.prototype.$axios = axios;
Vue.use(BootstrapVue)
Vue.use(BootstrapVueIcons)
Vue.use(VueSmoothScroll)
Vue.use(IconsPlugin)
Vue.use(Carousel3d);
Vue.use(Toasted);
new Vue({
  router,
  store,
  render: h => h(App)
}).$mount('#app')
