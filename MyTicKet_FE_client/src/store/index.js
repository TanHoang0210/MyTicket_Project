import Vue from 'vue'
import Vuex from 'vuex'
import router from '../router';
Vue.use(Vuex)

export default new Vuex.Store({
  state: {
    currentUser: null,
    accessToken: null,
    refreshToken: null,
    tokenExpiration: null,
    orderFormData: null,
  },
  getters: {
    currentUser: state => state.currentUser,
    accessToken: state => state.accessToken,
    refreshToken: state => state.refreshToken,
    tokenExpiration: state => state.tokenExpiration,
    orderFormData: state => state.orderFormData,
  },
  mutations: {
    setOrderData(state, orderFormData) {
      state.orderFormData = orderFormData;
    },
    setCurrentUser(state, user) {
      state.currentUser = user;
    },
     setTokens(state, { accessToken, refreshToken ,tokenExpiration}) {
      state.accessToken = accessToken;
      state.refreshToken = refreshToken;
      state.tokenExpiration = tokenExpiration;
    },
    logout(state){
      state.accessToken = null;
      state.refreshToken = null;
      state.tokenExpiration = null;
      state.currentUser = null;
      if (router.currentRoute.path !== '/') {
        // Chuyển hướng về trang chủ nếu không ở trên route hiện tại là '/'
        router.push('/');
      }else{
        location.reload()
      }
    }
  },
  actions: {
    logout({ commit }) {
      commit('logout');
    },
  },
  modules: {
  }
})
