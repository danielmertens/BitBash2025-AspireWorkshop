const HTMLWebpackPlugin = require("html-webpack-plugin");
const CopyWebpackPlugin = require('copy-webpack-plugin');

module.exports = (env) => {
  return {
    entry: "./src/index.js",
    devServer: {
      static: './public',
      port: env.PORT || 4001,
      allowedHosts: "all",
      proxy: [
        {
          context: ["/bff"],
          target:
            process.env.services__bff__https__0 ||
            process.env.services__bff__http__0,
          pathRewrite: { "^/bff": "" },
          secure: false,
        },
      ],
    },
    output: {
      path: `${__dirname}/dist`,
      filename: "bundle.js",
    },
    plugins: [
      new HTMLWebpackPlugin({
        template: "./src/index.html",
        favicon: "./src/favicon.ico",
      }),
      new CopyWebpackPlugin({
        patterns: [
          {from: "public", to: "public"}
        ]
      })
    ],
    module: {
      rules: [
        {
          test: /\.js$/,
          exclude: /node_modules/,
          use: {
            loader: "babel-loader",
            options: {
              presets: [
                "@babel/preset-env",
                ["@babel/preset-react", { runtime: "automatic" }],
              ],
            },
          },
        },
        {
          test: /\.css$/,
          exclude: /node_modules/,
          use: ["style-loader", "css-loader"],
        }
      ],
    },
  };
};
