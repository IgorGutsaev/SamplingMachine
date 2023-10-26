import { createApp } from 'vue'
import 'bootstrap'
import { library } from '@fortawesome/fontawesome-svg-core'
import { faPhone, faHouse, faSpinner, faPlus, faRemove } from '@fortawesome/free-solid-svg-icons'
import { FontAwesomeIcon } from '@fortawesome/vue-fontawesome'
import i18n from './i18n'
library.add(faPhone, faHouse, faSpinner, faPlus, faRemove)
import App from './App.vue'
import UI from './UI.vue'
import LoginService from './components/LoginService.vue'
import { createRouter, createWebHistory } from 'vue-router'

import mitt from 'mitt';
const emitter = mitt();

const router = createRouter({
    history: createWebHistory(),
    routes : [
        { path: '/', component: UI },
        { path: '/login', component: LoginService, alias: ['/l'], }
    ]
})

const app = createApp(App);
app.config.globalProperties.emitter = emitter;
app.component('font-awesome-icon', FontAwesomeIcon).use(i18n).use(router).mount('#app');