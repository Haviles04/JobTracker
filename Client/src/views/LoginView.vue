<script setup lang="ts">
import { ref, computed } from 'vue'
import axios from 'axios';
import { useRouter } from 'vue-router';

const router = useRouter();

const isAlreadyUser = ref<boolean>(false);
const email = ref<string>('')
const password = ref<string>('');
const errorMessage = ref<string>('');

const handleSubmit = async () => {
  const slug: string = isAlreadyUser.value ? 'login' : 'register'
  try {
    const response = await axios.post(`http://localhost:5130/${slug}`, {
      email: email.value,
      password: password.value
    })

    if (response.status === 200) {
      return router.push('/');
    }
  }
  catch (err: unknown) {
    if (axios.isAxiosError(err)) {
      const firstErrorKey = Object.keys(err.response?.data.errors)[0];
      errorMessage.value = err.response?.data.errors[firstErrorKey][0];
    } else {
      errorMessage.value = 'Unexpected Error, please try again';
    }
  }
}

const handleChange = () => {
  isAlreadyUser.value = !isAlreadyUser.value;
}

const headerText = computed<string>(() => {
  return isAlreadyUser.value ? "Login to view your jobs" : "Register now to start tracking jobs!";
})

const submitButtonText = computed<string>(() => {
  return isAlreadyUser.value ? "Login" : "Register";
})  
const alreadyHaveAccount = computed<string>(() => {
  return isAlreadyUser.value ? "Register now!" : "Already have an account? Login";
}) 
</script>

<template>
  <main>
    <div class="flex w-full justify-center flex-col items-center">
      <h1 class="text-2xl">Job Tracker</h1>
      <p>{{ headerText }}</p>
      <form class="text-center" @submit.prevent="handleSubmit">
        <label class="block" for="email">Email</label>
        <input class="border-2" type="text" v-model="email" id="email" required />
        <label class="block" for="password">Password</label>
        <input class="border-2" type="password" v-model="password" id="password" required/>
        <div class="w-full mt-2 flex justify-around flex-col">
          <button class="border-2 p-1 rounded" type="submit">{{submitButtonText}}</button>
          <button class="border-2 p-1 rounded">Forgot Password</button>
          <button class="block" @click="handleChange">{{ alreadyHaveAccount }}</button>
        </div>
      </form>
      <div>
        <p class="text-red-500">{{ errorMessage }}</p> 
      </div>
    </div>
    
  </main>
</template>
