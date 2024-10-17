namespace Bobo
{
    using Whetstone.ChatGPT.Models.Image;
    using System.Threading.Tasks;
    using System.Collections.Generic;
    using Whetstone.ChatGPT.Models;
    using Whetstone.ChatGPT;


    /// <summary>
    /// 
    /// </summary>
    public class ChatGptService
    {
        /// <summary>
        /// The chat GPT client
        /// </summary>
        private ChatGPTClient _chatGptClient;

        /// <summary>
        /// Initializes a new instance of the <see cref="ChatGptService"/> class.
        /// </summary>
        /// <param name="openaiApiKey">The openai API key.</param>
        public ChatGptService(string openaiApiKey)
        {
            _chatGptClient = new ChatGPTClient(openaiApiKey);
        }

        // After 2024-01-04, must use GPT-3.5 with ChatGPT35Models.
        // Turbo because Davinci003 (etc) is deprecated.
        // https://platform.openai.com/docs/deprecations/deprecation-history
        /// <summary>
        /// Creates the chat completion asynchronous.
        /// </summary>
        /// <param name="prompt">The prompt.</param>
        /// <returns></returns>
        public async Task<ChatGPTChatCompletionResponse> CreateChatCompletionAsync(string prompt)
        {
            var _gptRequest = new ChatGPTChatCompletionRequest
            {
                Model = ChatGPT35Models.Turbo,
                Messages = new List<ChatGPTChatCompletionMessage>()
                {
                    new ChatGPTChatCompletionMessage()
                    {
                        Role = ChatGPTMessageRoles.System,
                        Content = "You are a helpful assistant."
                    },
                    new ChatGPTChatCompletionMessage()
                    {
                        Role = ChatGPTMessageRoles.User,
                        Content = prompt
                    },
                },
                Temperature = 0.9f,
                MaxTokens = 500,
            };
            return await _chatGptClient.CreateChatCompletionAsync(_gptRequest);
        }

        // After 2024-01-04, must use GPT-3.5 with ChatGPT35Models.Turbo
        /// <summary>
        /// Streams the chat completion asynchronous.
        /// </summary>
        /// <param name="prompt">The prompt.</param>
        /// <returns></returns>
        public IAsyncEnumerable<ChatGPTChatCompletionStreamResponse> StreamChatCompletionAsync(string prompt)
        {
            var _completionRequest = new ChatGPTChatCompletionRequest
            {
                Model = ChatGPT35Models.Turbo,
                Messages = new List<ChatGPTChatCompletionMessage>()
                {
                    new ChatGPTChatCompletionMessage()
                    {
                        Role = ChatGPTMessageRoles.System,
                        Content = "You are a helpful assistant."
                    },
                    new ChatGPTChatCompletionMessage()
                    {
                        Role = ChatGPTMessageRoles.User,
                        Content = prompt
                    },
                },
                Temperature = 0.9f,
                MaxTokens = 500,
            };
            return _chatGptClient.StreamChatCompletionAsync(_completionRequest);
        }

        /// <summary>
        /// Creates the image asynchronous.
        /// </summary>
        /// <param name="prompt">The prompt.</param>
        /// <returns></returns>
        public async Task<byte[]> CreateImageAsync(string prompt)
        {
            ChatGPTCreateImageRequest _imageRequest = new()
            {
                Prompt = prompt,
                Size = CreatedImageSize.Size1024,
                ResponseFormat = CreatedImageFormat.Base64
            };

            byte[] _imageBytes = null;
            var _imageResponse = await _chatGptClient.CreateImageAsync(_imageRequest);
            if (_imageResponse != null)
            {
                var _imageData = _imageResponse.Data?[0];
                if (_imageData != null)
                {
                    _imageBytes = await _chatGptClient.DownloadImageAsync(_imageData);
                }
            }
            return _imageBytes;
        }
    }
}