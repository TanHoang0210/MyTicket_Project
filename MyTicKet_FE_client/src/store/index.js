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
    countdownTime:300,
  },
  getters: {
    currentUser: state => state.currentUser,
    accessToken: state => state.accessToken,
    refreshToken: state => state.refreshToken,
    tokenExpiration: state => state.tokenExpiration,
    orderFormData: state => state.orderFormData,
    countdownTime: state => state.countdownTime
  },
  mutations: {
    setOrderData(state, orderFormData) {
      state.orderFormData = orderFormData;
    },
    setCoutDownOrder(state,countdownTime){
      state.countdownTime = countdownTime;
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
      sessionStorage.removeItem('accessToken'),
      sessionStorage.removeItem('refreshToken'),
      sessionStorage.removeItem('currentUser'),
      sessionStorage.removeItem('tokenExpiration'),
      state.accessToken = null;
      state.refreshToken = null;
      state.tokenExpiration = null;
      state.currentUser = null;
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
