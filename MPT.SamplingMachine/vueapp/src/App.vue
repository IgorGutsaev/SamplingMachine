<template>
    <!--   <img alt="Vue logo" src="./assets/logo.png"> -->
    <Sampling msg="OgmentO sampling machine" />
</template>

<script>
    import Sampling from './components/SamplingPage.vue' 

    export default {
      name: 'App',
      components: {
        Sampling
      }
    }
</script>

<style>
    @import url('css/style.css');

#app {
  font-family: Avenir, Helvetica, Arial, sans-serif;
  -webkit-font-smoothing: antialiased;
  -moz-osx-font-smoothing: grayscale;
  text-align: center;
  color: #2c3e50;
  margin-top: 60px;
}

@import'~bootstrap/dist/css/bootstrap.css'
</style>
