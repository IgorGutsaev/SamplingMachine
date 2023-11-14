<template>
    <div :class="keyboardClass"></div>
</template>

<script>
    import Keyboard from "simple-keyboard";
    import "simple-keyboard/build/css/index.css";

    export default {
        name: "SimpleKeyboard",
        props: {
            keyboardClass: {
                default: "simple-keyboard",
                type: String
            },
            input: {
                type: String
            },
            maxLength: {
                type: Number,
                def: 10
            }
        },
        data: () => ({
            keyboard: null
        }),
        mounted() {
            this.keyboard = new Keyboard(this.keyboardClass, {
                onChange: this.onChange,
                onKeyPress: this.onKeyPress,
                layout: { default: ["1 2 3", "4 5 6", "7 8 9", "0 {bksp}"] },
                display: {
                    '{bksp}': '←'
                },
                maxLength: this.maxLength
            });
        },
        methods: {
            onChange(input) {
                this.$emit("onChange", input);
            },
            onKeyPress(button) {
                this.$emit("onKeyPress", button);

                /**
                 * If you want to handle the shift and caps lock buttons
                 */
                if (button === "{shift}" || button === "{lock}") this.handleShift();
            },
            handleShift() {
                let currentLayout = this.keyboard.options.layoutName;
                let shiftToggle = currentLayout === "default" ? "shift" : "default";

                this.keyboard.setOptions({
                    layoutName: shiftToggle
                });
            }
        },
        watch: {
            input(input) {
                this.keyboard.setInput(input);
            }
        }
    };
</script>

<!-- Add "scoped" attribute to limit CSS to this component only -->
<style scoped>
</style>
